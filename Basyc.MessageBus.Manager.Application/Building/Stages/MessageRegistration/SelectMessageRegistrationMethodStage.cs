using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi;
using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration
{
	public class SelectMessageRegistrationMethodStage : BuilderStageBase
	{
		public SelectMessageRegistrationMethodStage(IServiceCollection services) : base(services)
		{
			services.AddSingleton<IBusManagerApplication, BusManagerApplication>();
		}

		public RegisterMessagesFromFluentApiStage RegisterMessagesViaFluentApi()
		{
			services.AddSingleton<IRequesterSelector, RequesterSelector>();
			services.AddSingleton<IRequester, InMemoryDelegateRequester>();
			services.AddSingleton<InMemoryDelegateRequester>();
			services.AddSingleton<IDomainInfoProvider, FluentApiDomainInfoProvider>();

			return new RegisterMessagesFromFluentApiStage(services);
		}

		public RegisterMessagesFromAssemblyStage RegisterMessagesFromAssembly(params Assembly[] assembliesToScan)
		{
			return new RegisterMessagesFromAssemblyStage(services, assembliesToScan);
		}
	}
}