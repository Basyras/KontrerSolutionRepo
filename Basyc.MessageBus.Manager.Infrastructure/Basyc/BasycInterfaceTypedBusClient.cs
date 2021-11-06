using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Kontrer.Shared.MessageBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
    public class BasycInterfaceTypedBusClient : IBusClient
    {
        private readonly IRequestInfoTypeStorage requestInfoTypeStorage;
        private readonly IResponseFormatter responseFormatter;

        public BasycInterfaceTypedBusClient(IMessageBusManager messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
        {
            MessageBusManager = messageBusManager;
            this.requestInfoTypeStorage = requestInfoTypeStorage;
            this.responseFormatter = responseFormatter;
        }

        public IMessageBusManager MessageBusManager { get; }

        public async Task<RequestResult> TrySendRequest(Request request)
        {
            var paramValues = request.Parameters.Select(x => x.Value).ToArray();
            var requestType = requestInfoTypeStorage.GetRequestType(request.RequestInfo);

            var requestInstance = Activator.CreateInstance(requestType, paramValues);
            var stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                if (request.RequestInfo.HasResponse)
                {
                    var response = await MessageBusManager.RequestAsync(requestType, requestInstance, request.RequestInfo.ResponseType);
                    stopWatch.Stop();
                    return new RequestResult(false, responseFormatter.Format(response), string.Empty, stopWatch.Elapsed);
                }
                else
                {
                    RequestResult result = null;
                    await MessageBusManager.SendAsync(requestType, requestInstance)
                    .ContinueWith(x =>
                    {
                        stopWatch.Stop();
                        string errorMessage = x.Exception != null ? x.Exception.Message : string.Empty;
                        result = new RequestResult(x.IsFaulted, errorMessage, stopWatch.Elapsed);
                    });

                    return result;
                }
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                return new RequestResult(true, ex.Message, stopWatch.Elapsed);
            }
        }
    }
}