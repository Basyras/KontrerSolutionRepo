using Basyc.DependencyInjection;
using Basyc.MessageBus.Client.Diagnostics;
using Basyc.MessageBus.Client.Diagnostics.Sinks;
using Basyc.MessageBus.Client.RequestResponse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
			return RegisterBasycTypedHandlersCustom(typeof(THandlerAssemblyMarker).Assembly);
		}

		private BusClientSetupProviderStage RegisterBasycTypedHandlers(params Assembly[] assembliesToScan)
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

		public BusClientSetupProviderStage RegisterBasycTypedHandlersCustom(params Assembly[] assembliesToScan)
		{

			foreach (var assembly in assembliesToScan)
			{
				Type[] typesInAssembly = assembly.GetTypes();
				var handlerTypesInAssembly = typesInAssembly.Where(x => x.IsAssignableToGenericType(typeof(IMessageHandler<>)));
				foreach (var handlerType in handlerTypesInAssembly)
				{
					var serviceType = typeof(IMessageHandler<>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<>)));
					services.AddScoped(serviceType, serviceProvider => CreateHandlerWithDecoratedLoggerT(handlerType, serviceProvider));
				}

				var handlerTypesInAssembly2 = typesInAssembly.Where(x => x.IsAssignableToGenericType(typeof(IMessageHandler<,>)));
				foreach (var handlerType in handlerTypesInAssembly2)
				{
					var serviceType = typeof(IMessageHandler<,>).MakeGenericType(GenericsHelper.GetTypeArgumentsFromParent(handlerType, typeof(IMessageHandler<,>)));
					services.AddScoped(serviceType, serviceProvider => CreateHandlerWithDecoratedLoggerT(handlerType, serviceProvider));
				}

			}
			return new BusClientSetupProviderStage(services);
		}

		private static object CreateHandlerWithDecoratedLoggerT(Type handlerType, IServiceProvider services)
		{
			var ctor = handlerType.GetConstructors().First();
			var ctorParams = ctor.GetParameters();
			object[] ctorArguments = new object[ctorParams.Length];
			for (int paramIndex = 0; paramIndex < ctorParams.Length; paramIndex++)
			{
				ParameterInfo ctorParam = ctorParams[paramIndex];
				if (ctorParam.ParameterType == typeof(ILogger))
				{
					var logSinks = services.GetServices<ILogSink>().ToArray();
					ctorArguments[paramIndex] = new BusHandlerLogger((ILogger)services.GetRequiredService(ctorParam.ParameterType), logSinks, handlerType.Name);
					continue;
				}
				//if (ctorParam.ParameterType == typeof(ILogger<>).MakeGenericType(handlerType))
				if (ctorParam.ParameterType.IsAssignableToGenericType(typeof(ILogger<>)))
				{
					var originalLoggerGenericArgument = ctorParam.ParameterType.GetTypeArgumentsFromParent(typeof(ILogger<>))[0];
					var logSinks = services.GetServices<ILogSink>().ToArray();
					var decoLoggerType = typeof(BusHandlerLogger<>).MakeGenericType(originalLoggerGenericArgument);
					var decoLoggerCtor = decoLoggerType.GetConstructor(new Type[] { typeof(ILogger), typeof(IEnumerable<ILogSink>) });
					var decoLogger = decoLoggerCtor.Invoke(new object[] { services.GetRequiredService(ctorParam.ParameterType), logSinks });
					ctorArguments[paramIndex] = decoLogger;
					continue;
				}
				ctorArguments[paramIndex] = services.GetRequiredService(ctorParam.ParameterType);
			}
			var handlerInstance = ctor.Invoke(ctorArguments);
			return handlerInstance;
		}
	}
}
