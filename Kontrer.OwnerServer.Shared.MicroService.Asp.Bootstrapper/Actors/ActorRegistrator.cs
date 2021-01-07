using Dapr.Actors;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.MicroService.Asp.Bootstrapper.Actors
{
    public class ActorRegistrator
    {
        private readonly IMicroserviceProvider microserviceProvider;

        public ActorRegistrator(IMicroserviceProvider microserviceProvider)
        {
            this.microserviceProvider = microserviceProvider;
        }

        public void RegisterActors<TStartup>()
        {
            RegisterActors(typeof(TStartup).Namespace + "/Actors");
        }

        public void RegisterActors(string actorsNamespace)
        {

            var actorTypes = Assembly.GetEntryAssembly().DefinedTypes.Where(x => x.IsClass && x.ImplementedInterfaces.Contains(typeof(IActor)) && x.Namespace.StartsWith(actorsNamespace));
            foreach (var actor in actorTypes)
            {
                microserviceProvider.RegisterActor(actor);
            }
        }
    }
}
