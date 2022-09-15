using Basyc.MessageBus.Manager;
using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration;
using Basyc.MessageBus.Manager.Presentation.BlazorLibrary;
using MudBlazor.Services;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class IServicesBusMangerExtensions
	{
		public static BusManagerApplicationBuilder AddBasycBusBlazorUI(this IServiceCollection services)
		{
			services.AddMudServices();
			services.AddSingleton<BusManagerJSInterop>();
			return services.AddMessageManager();
		}
	}
}