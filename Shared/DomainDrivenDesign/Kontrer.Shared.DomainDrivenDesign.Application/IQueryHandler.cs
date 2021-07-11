using Kontrer.Shared.MessageBus.RequestResponse;
using Kontrer.Shared.DomainDrivenDesign.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.Shared.DomainDrivenDesign.Application
{
    public interface IQueryHandler<TQuery, TReponse> : IRequestHandler<TQuery, TReponse>
      where TQuery : class, IQuery<TReponse>
      where TReponse : class
    {
    }
}