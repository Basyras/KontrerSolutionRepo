using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace Basyc.MessageBus.Manager.Presentation.BlazorLibrary.Pages.Requests;

public partial class BusManager
{
	[Inject]
	public IDomainInfoProviderManager busManager { get; private set; }
	[Inject]
	public ITypedMessageBusClient MessageBusManager { get; private set; }
	[Inject]
	public IDialogService DialogService { get; private set; }
	[Inject]
	public IRequestManager RequestClient { get; private set; }
	[Inject]
	public BusManagerJSInterop BusManagerJSInterop { get; private set; }

	public List<DomainItemViewModel> DomainInfoViewModel { get; private set; } = new List<DomainItemViewModel>();

	public RequestItemViewModel SelectedRequestViewModel
	{
		get => selectedRequestViewModel;
		private set
		{
			if (selectedRequestViewModel is not null)
			{
				selectedRequestViewModel.IsSelected = false;
			}
			selectedRequestViewModel = value;
			if (value is null)
			{
				return;
			}
			selectedRequestViewModel.IsSelected = true;
			resultHistory.TryAdd(value, new List<RequestResult>());
			selectedResult = null;
		}
	}

	private RequestItemViewModel selectedRequestViewModel;
	private RequestResult selectedResult;
	private readonly Dictionary<RequestItemViewModel, List<RequestResult>> resultHistory = new Dictionary<RequestItemViewModel, List<RequestResult>>();

	protected override void OnInitialized()
	{
		DomainInfoViewModel = busManager.GetDomainInfos()
			.Select(domainInfo => new DomainItemViewModel(domainInfo, domainInfo.Requests
				.Select(requestInfo => new RequestItemViewModel(requestInfo))
				.OrderBy(x => x.RequestInfo.RequestType)))
			.ToList();

		base.OnInitialized();
	}
	protected override async Task OnParametersSetAsync()
	{
		await BusManagerJSInterop.ApplyChangesToIndexHtml();
		await base.OnParametersSetAsync();
	}
	protected override async Task OnInitializedAsync()
	{
		await BusManagerJSInterop.ApplyChangesToIndexHtml();
		await base.OnInitializedAsync();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await BusManagerJSInterop.ApplyChangesToIndexHtml();
		}
		await base.OnAfterRenderAsync(firstRender);
	}

	public async Task SendMessage(RequestItemViewModel requestItem)
	{
		var requestInfo = requestItem.RequestInfo;
		List<Parameter> parameters = new List<Parameter>(requestInfo.Parameters.Count);
		for (int i = 0; i < requestInfo.Parameters.Count; i++)
		{
			var paramInfo = requestInfo.Parameters[i];
			var paramStringValue = requestItem.ParameterValues[i];
			var castedParamValue = ParseParamInputValue(paramStringValue, paramInfo);
			parameters.Add(new Parameter(paramInfo, castedParamValue));
		}
		Request request = new Request(requestInfo, parameters);
		var requestResult = RequestClient.StartRequest(request);
		requestItem.LastResult = requestResult;
		resultHistory.TryAdd(requestItem, new List<RequestResult>());
		var requestHistory = resultHistory[requestItem];
		requestHistory.Add(requestResult);

	}

	private static object ParseParamInputValue(string paramStringValue, ParameterInfo parameterInfo)
	{
		if (paramStringValue == "@null")
		{
			return null;
		}

		if (paramStringValue == String.Empty)
		{
			return parameterInfo.Type.GetDefaultValue();
		}

		if (parameterInfo.Type == typeof(string))
		{
			return paramStringValue;
		}

		TypeConverter converter = TypeDescriptor.GetConverter(parameterInfo.Type);
		object castedParam;
		if (converter.CanConvertFrom(typeof(string)))
		{
			castedParam = converter.ConvertFromInvariantString(paramStringValue);
			return castedParam;
		}

		TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(string));
		if (converter2.CanConvertFrom(parameterInfo.Type))
		{
			castedParam = converter2.ConvertFromInvariantString(paramStringValue);
			return castedParam;
		}

		try
		{
			castedParam = Convert.ChangeType(paramStringValue, parameterInfo.Type);
			return castedParam;
		}
		catch
		{
			//Try change type, if fails, use json Deserialize
		}

		castedParam = JsonSerializer.Deserialize(paramStringValue, parameterInfo.Type);
		return castedParam;
	}

	public static string GetColor(string textInput, int saturation, int saturationRandomness = 0)
	{
		int seed = textInput.Select(x => (int)x).Sum();
		var random = new Random(seed);

		var remainingColours = new List<int>(3) { 0, 1, 2 };
		int[] colours = new int[3];
		int firstIndex = random.Next(0, 2);
		int randomSaturationToApply = random.Next(0, saturationRandomness);
		colours[remainingColours[firstIndex]] = 255 - randomSaturationToApply;
		remainingColours.RemoveAt(firstIndex);

		int secondIndex = remainingColours[random.Next(0, 1)];
		randomSaturationToApply = random.Next(0, saturationRandomness);
		colours[remainingColours[secondIndex]] = saturation - randomSaturationToApply;
		remainingColours.RemoveAt(secondIndex);

		int flexibleSaturation = random.Next(saturation, 255);
		randomSaturationToApply = random.Next(0, saturationRandomness);
		colours[remainingColours[0]] = flexibleSaturation - randomSaturationToApply;

		StringBuilder stringBuilder = new StringBuilder(6);
		stringBuilder.Append("#");
		stringBuilder.Append(colours[0].ToString("X2"));
		stringBuilder.Append(colours[1].ToString("X2"));
		stringBuilder.Append(colours[2].ToString("X2"));
		string finalColor = stringBuilder.ToString();
		return finalColor;
	}
}
