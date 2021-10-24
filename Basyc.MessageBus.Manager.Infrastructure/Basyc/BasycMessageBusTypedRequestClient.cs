using Basyc.MessageBus.Manager.Application;
using Kontrer.Shared.MessageBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure.Basyc
{
    public class BasycMessageBusTypedRequestClient : IRequestClient
    {
        private readonly IRequestInfoTypeStorage requestInfoTypeStorage;

        public BasycMessageBusTypedRequestClient(IMessageBusManager messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage)
        {
            MessageBusManager = messageBusManager;
            this.requestInfoTypeStorage = requestInfoTypeStorage;
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
                    return new RequestResult(false, response, string.Empty, stopWatch.Elapsed);
                }
                else
                {
                    RequestResult result = null;
                    await MessageBusManager.SendAsync(requestType, requestInstance)
                    .ContinueWith(x =>
                    {
                        stopWatch.Stop();
                        result = new RequestResult(x.IsFaulted, x.Exception.Message, stopWatch.Elapsed);
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