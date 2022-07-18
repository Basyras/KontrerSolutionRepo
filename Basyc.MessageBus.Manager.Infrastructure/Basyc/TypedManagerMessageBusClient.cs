using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
	public class TypedManagerMessageBusClient : IManagerMessageBusClient
	{
		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IResponseFormatter responseFormatter;

		public TypedManagerMessageBusClient(ITypedMessageBusClient messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
		{
			MessageBusManager = messageBusManager;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
		}

		public ITypedMessageBusClient MessageBusManager { get; }

		public async Task<RequestResult> TrySendRequest(Request request)
		{
			var paramValues = request.Parameters.Select(x => x.Value).ToArray();
			var requestType = requestInfoTypeStorage.GetRequestType(request.RequestInfo);
			var requestObject = Activator.CreateInstance(requestType, paramValues);
			var stopWatch = new Stopwatch();
			var requestTime = DateTime.Now;
			try
			{
				stopWatch.Start();
				if (request.RequestInfo.HasResponse)
				{
					var response = await MessageBusManager.RequestAsync(requestType, requestObject, request.RequestInfo.ResponseType);
					stopWatch.Stop();
					return new RequestResult(request, false, responseFormatter.Format(response), string.Empty, requestTime, stopWatch.Elapsed);
				}
				else
				{
					RequestResult result = null;
					await MessageBusManager.SendAsync(requestType, requestObject)
					.ContinueWith(x =>
					{
						stopWatch.Stop();
						string errorMessage = string.Empty;
						if (x.IsFaulted)
						{
							if (x.Exception is AggregateException aggregateException)
							{
								errorMessage = aggregateException.InnerExceptions.Select(x => x.Message).Aggregate((x, y) => $"{x},\n{y}");
							}
							else
							{
								errorMessage = x.Exception != null ? x.Exception.Message : string.Empty;
							}
						}
						result = new RequestResult(request, x.IsFaulted, errorMessage, requestTime, stopWatch.Elapsed);
					});

					return result;
				}
			}
			catch (Exception ex)
			{
				stopWatch.Stop();
				return new RequestResult(request, true, ex.Message, requestTime, stopWatch.Elapsed);
			}
		}
	}
}