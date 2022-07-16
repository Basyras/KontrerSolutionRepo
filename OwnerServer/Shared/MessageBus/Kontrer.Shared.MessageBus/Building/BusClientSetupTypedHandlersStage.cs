using Basyc.DependencyInjection;
using Basyc.MessageBus.Client.RequestResponse;
using Basyc.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Basyc.MessageBus.Client.Building
{
	public class BusClientSetupTypedHandlersStage : BuilderStageBase
	{
		public BusClientSetupTypedHandlersStage(IServiceCollection services, MessageTypeOptions messageTypeOptions) : base(services)
		{
			MessageTypeOptions = messageTypeOptions;
		}

		public MessageTypeOptions MessageTypeOptions { get; }

		public BusClientSelectClientProviderStage NoHandlers()
		{
			return new BusClientSelectClientProviderStage(services, MessageTypeOptions);
		}

		public BusClientSelectClientProviderStage RegisterBasycTypedHandlers<THandlerAssemblyMarker>()
		{
			return RegisterBasycTypedHandlers(typeof(THandlerAssemblyMarker).Assembly);
		}

		public BusClientSelectClientProviderStage RegisterBasycTypedHandlers(params Assembly[] assembliesToScan)
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

			return new BusClientSelectClientProviderStage(services, MessageTypeOptions);
		}


	}
}
