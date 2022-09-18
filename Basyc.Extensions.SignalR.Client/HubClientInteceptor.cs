using Castle.DynamicProxy;
using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection;

namespace Basyc.Extensions.SignalR.Client
{
	internal class HubClientInteceptor : IInterceptor
	{
		private readonly Dictionary<MethodInfo, Func<object?[], Task>> methodInfoToImplementationMap = new();

		public HubClientInteceptor(HubConnection connection, Type hubClientInterfaceType)
		{
			CreateInteceptorsForPublicMethods(connection, hubClientInterfaceType);
		}

		private void CreateInteceptorsForPublicMethods(HubConnection connection, Type hubClientInterfaceType)
		{
			foreach (var methodInfo in hubClientInterfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
			{
				CheckMethodCanBeIntercepted(methodInfo);
				Func<object?[], Task> func = (arguments) =>
				{
					TryGetCancelToken(arguments, out var cancelToken);
					var argumentsToSend = GetArgumentsToSend(arguments);
					return connection.SendCoreAsync(methodInfo.Name, argumentsToSend, cancelToken);
				};
				methodInfoToImplementationMap.Add(methodInfo, func);
			}
		}

		public void Intercept(IInvocation invocation)
		{
			var implementedMethod = methodInfoToImplementationMap[invocation.Method];
			invocation.ReturnValue = implementedMethod.Invoke(invocation.Arguments);
		}

		private static bool TryGetCancelToken(object?[] arguments, out CancellationToken cancelToken)
		{
			var cancelTokenObject = arguments.FirstOrDefault(x =>
			{
				if (x is null)
					return false;

				return x.GetType() == typeof(CancellationToken);
			});
			if (cancelTokenObject is null)
			{
				cancelToken = default;
				return false;
			}
			cancelToken = (CancellationToken)cancelTokenObject;
			return true;
		}

		private static object?[] GetArgumentsToSend(object?[] arguments)
		{
			var argumentsToSend = arguments.Where(x =>
			{
				if (x is null)
					return true;

				return x.GetType() != typeof(CancellationToken);
			}).ToArray();
			return argumentsToSend;
		}

		private static void CheckMethodCanBeIntercepted(MethodInfo methodInfo)
		{
			if (methodInfo.ReturnType != typeof(void)
				&& methodInfo.ReturnType != typeof(Task))
			{
				throw new ArgumentException($"You can provide interface that has only methods that have return values: {typeof(void).Name} or {typeof(Task).Name}");
			}
		}
	}
	internal class HubClientInteceptor<THubClient> : HubClientInteceptor
	{
		public HubClientInteceptor(HubConnection connection) : base(connection, typeof(THubClient))
		{
		}
	}


}
