using Basyc.DependencyInjection;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientSetupHandlersStage : BuilderStageBase
	{
		public BusClientSetupHandlersStage(IServiceCollection services) : base(services)
		{
		}

		public BusClientSetupProviderStage NoHandlers()
		{
			return new BusClientSetupProviderStage(services);
		}

		public BusClientSetupProviderStage RegisterBasycTypedHandlers<THandlerAssemblyMarker>()
		{
			return RegisterBasycTypedHandlers(typeof(THandlerAssemblyMarker).Assembly);
		}

		public BusClientSetupProviderStage RegisterBasycTypedHandlers(params Assembly[] assembliesToScan)
		{
			services.Scan(scan =>
			scan.FromAssemblies(assembliesToScan)
			.AddClasses(classes => classes.AssignableTo(typeof(IMessageHandler<>)))
			.As(handler => new Type[1]
			{
				typeof(IMessageHandler<>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handler, typeof(IMessageHandler<>)))
			})
			.WithScopedLifetime()

			.AddClasses(classes => classes.AssignableTo(typeof(IMessageHandler<,>)))
			.As(handler => new Type[1]
			{
				typeof(IMessageHandler<,>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handler, typeof(IMessageHandler<,>)))
			})
			.WithScopedLifetime());

			return new BusClientSetupProviderStage(services);
		}


	}
}
