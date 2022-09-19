using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection;

namespace Basyc.Extensions.SignalR.Client
{
	public static class OnMultipleExtension
	{
		public static void OnMultiple<TMethodsServerCanCall>(HubConnection hubConnection, TMethodsServerCanCall serverMethods)
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
			//var genericArgType = typeof(TMethodsServerCanCall);
			//Type targetType = serverMethods!.GetType();
			Type targetType = typeof(TMethodsServerCanCall); //Ignore methods that are not specified in interfac (They should not be called from server + server hub does even see them)
			var filteredMethods = targetType
				.GetMethodsRecursive(BindingFlags.Public | BindingFlags.Instance)
				.Where<MethodInfo>(methodInfo =>
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
