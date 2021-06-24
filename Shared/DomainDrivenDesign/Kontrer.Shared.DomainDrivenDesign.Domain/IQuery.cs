﻿using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.DomainDrivenDesign.Domain
{
    public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : class
    {
    }
}