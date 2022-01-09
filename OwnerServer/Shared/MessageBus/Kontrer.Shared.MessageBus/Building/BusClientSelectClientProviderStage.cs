using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client.Building
{
    public class BusClientSelectClientProviderStage : BuilderStageBase
    {
        public BusClientSelectClientProviderStage(IServiceCollection services) : base(services)
        {

        }


    }
}
