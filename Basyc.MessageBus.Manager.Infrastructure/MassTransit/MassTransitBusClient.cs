using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using Kontrer.Shared.MessageBus;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure.MassTransit
{
    public class MassTransitBusClient : BasycInterfaceTypedBusClient, IBusClient
    {
        public MassTransitBusClient(IMessageBusManager messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
            : base(messageBusManager, requestInfoTypeStorage, responseFormatter)
        {
        }

        //private readonly IBus bus;
        //private readonly IRequestInfoTypeStorage requestInfoTypeStorage;

        //public MassTransitRequestClient(IBus bus, IRequestInfoTypeStorage requestInfoTypeStorage)
        //{
        //    this.bus = bus;
        //    this.requestInfoTypeStorage = requestInfoTypeStorage;
        //}
        //public async Task<RequestResult> TrySendRequest(Request request)
        //{
        //    var paramValues = request.Parameters.Select(x => x.Value).ToArray();
        //    var requestType = requestInfoTypeStorage.GetRequestType(request.RequestInfo);
        //    var requestInstance = Activator.CreateInstance(requestType, paramValues);
        //    throw new NotImplementedException();
        //}
    }
}