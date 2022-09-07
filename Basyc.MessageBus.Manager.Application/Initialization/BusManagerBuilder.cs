using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager
{
	public class BusManagerBuilder
	{
		public readonly IServiceCollection services;

		public BusManagerBuilder(IServiceCollection services)
		{
			this.services = services;
			services.AddSingleton<IBusManagerApplication, BusManagerApplication>();
		}

		public BusManagerBuilder AddProvider<TDomainProvider>() where TDomainProvider : class, IDomainInfoProvider
		{
			services.AddSingleton<IDomainInfoProvider, TDomainProvider>();
			return this;
		}

		public BusManagerBuilder SelectRequester<TRequester>() where TRequester : class, IRequester
		{
			services.AddSingleton<IRequester, TRequester>();
			return this;
		}
	}
}