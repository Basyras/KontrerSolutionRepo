﻿using Basyc.MessageBus.Client;
using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Infrastructure.MassTransit;

public class MassTransitBusClient : TypedRequester, IRequester
{
    public MassTransitBusClient(ITypedMessageBusClient messageBusManager, IRequestInfoTypeStorage requestInfoTypeStorage, IResponseFormatter responseFormatter)
        : base(messageBusManager, requestInfoTypeStorage, responseFormatter)
    {
    }
}
