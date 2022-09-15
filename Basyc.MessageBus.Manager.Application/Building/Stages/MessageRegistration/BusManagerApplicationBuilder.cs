using Basyc.DependencyInjection;
using Basyc.Diagnostics.Receiving.Abstractions;
using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Application.Requesting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration
{
	public class BusManagerApplicationBuilder : BuilderStageBase
	{
		public BusManagerApplicationBuilder(IServiceCollection services) : base(services)
		{
			services.AddSingleton<IDomainInfoProviderManager, DomainInfoProviderManager>();
		}

		public RegisterMessagesFromFluentApiStage RegisterMessagesViaFluentApi()
		{
			services.AddSingleton<IRequesterSelector, RequesterSelector>();
			services.AddSingleton<IRequester, InMemoryDelegateRequester>();
			services.AddSingleton<InMemoryDelegateRequester>();
			services.AddSingleton<ILogSource, InMemoryLogSource>();
			services.AddSingleton<InMemoryLogSource>(x => (InMemoryLogSource)x.GetServices<ILogSource>().First(x => x is InMemoryLogSource));
			services.AddSingleton<IDomainInfoProvider, FluentApiDomainInfoProvider>();

			return new RegisterMessagesFromFluentApiStage(services);
		}

		public RegisterMessagesFromAssemblyStage RegisterMessagesFromAssembly(params Assembly[] assembliesToScan)
		{
			return new RegisterMessagesFromAssemblyStage(services, assembliesToScan);
		}
	}
}