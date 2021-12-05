using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basyc.DomainDrivenDesign.Domain;
using Basyc.MessageBus.RequestResponse;

namespace Basyc.DomainDrivenDesign.Application
{
    public interface IQueryHandler<TQuery, TReponse> : IRequestHandler<TQuery, TReponse>
      where TQuery : class, IQuery<TReponse>
      where TReponse : class
    {
    }
}