using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Basyc.MessageBus.Shared;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
	public class BasycTypedMessageBusRequester : IRequester
	{
		public const string BasycTypedMessageBusRequesterUniqueName = nameof(BasycTypedMessageBusRequester);


		private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
		private readonly IResponseFormatter responseFormatter;
		private readonly ITypedMessageBusClient typedMessageBusClient;

		public BasycTypedMessageBusRequester(ITypedMessageBusClient typedMessageBusClient, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
		{
			this.typedMessageBusClient = typedMessageBusClient;
			this.requestInfoTypeStorage = requestInfoTypeStorage;
			this.responseFormatter = responseFormatter;
		}


		public string UniqueName => BasycTypedMessageBusRequesterUniqueName;

		public void StartRequest(RequestResult result)
		{
			var requestType = requestInfoTypeStorage.GetRequestType(result.Request.RequestInfo);
			var paramValues = result.Request.Parameters.Select(x => x.Value).ToArray();
			var requestObject = Activator.CreateInstance(requestType, paramValues);

			Task.Run(async () =>
			{
				var stopWatch = new Stopwatch();
				try
				{
					stopWatch.Start();
					if (result.Request.RequestInfo.HasResponse)
					{
						var response = await typedMessageBusClient.RequestAsync(requestType, requestObject, result.Request.RequestInfo.ResponseType);
						stopWatch.Stop();

						if (response.Value is ErrorMessage error)
						{
							result.Fail(stopWatch.Elapsed, error.Message);
						}
						else
						{
							var resultObject = response.AsT0;
							result.Complete(stopWatch.Elapsed, responseFormatter.Format(resultObject));
						}
					}
					else
					{
						await typedMessageBusClient.SendAsync(requestType, requestObject)
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
								result.Fail(stopWatch.Elapsed, errorMessage);
							}
							result.Complete(stopWatch.Elapsed);
						});
					}
				}
				catch (Exception ex)
				{
					stopWatch.Stop();
					result.Fail(stopWatch.Elapsed, ex.Message);
				}
			});
		}
	}
}