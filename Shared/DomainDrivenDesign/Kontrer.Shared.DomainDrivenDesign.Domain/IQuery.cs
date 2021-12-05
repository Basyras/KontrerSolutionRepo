using Basyc.MessageBus.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.DomainDrivenDesign.Domain
{
    public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : class
    {
    }
}