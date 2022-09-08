using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;

namespace Basyc.MessageBus.Manager.Infrastructure.MassTransit;

public class MassTransitRequester : BasycTypedMessageBusRequester, IRequester
{
	public MassTransitRequester(ITypedMessageBusClient messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
		: base(messageBusManager, requestInfoTypeStorage, responseFormatter)
	{
	}
}
