using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection;

namespace Basyc.Extensions.SignalR.Client
{
	public static class HubListener
	{
		public static void ForwardTo<TMethodsServerCanCall>(HubConnection hubConnection, TMethodsServerCanCall serverMethods)
		{
			var methodInfos = FilterMethods(serverMethods);
			foreach (var methodInfo in methodInfos)
			{
				Type[] parameterTypes = methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();
				if (methodInfo.ReturnType == typeof(Task))
				{
					hubConnection.On(methodInfo.Name, parameterTypes, (arguments) => (Task)methodInfo.Invoke(serverMethods, arguments)!);
					continue;
				}

				if (methodInfo.ReturnType == typeof(void))
				{
					hubConnection.On(methodInfo.Name, parameterTypes, (arguments) =>
					{
						methodInfo.Invoke(serverMethods, arguments);
						return Task.CompletedTask;
					});
					continue;
				}

				throw new ArgumentException("Class must not contain public methods with different return types than void and Task");
			}
		}

		private static readonly MethodInfo[] MethodsToIgnore = new object().GetType().GetMethodsRecursive(BindingFlags.Public | BindingFlags.Instance);

		private static MethodInfo[] FilterMethods<TMethodsServerCanCall>(TMethodsServerCanCall serverMethods)
		{
			var filteredMethods = serverMethods!.GetType()
				.GetMethodsRecursive(BindingFlags.Public | BindingFlags.Instance)
				.Where(methodInfo =>
				{
					if (methodInfo.IsSpecialName)
						return false;
					return MethodsToIgnore.All(methodToIgnore => methodToIgnore.Name != methodInfo.Name);
				})
				.ToArray();

			return filteredMethods;

		}
	}
}
