using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi
{
	public class FluntSetupHandlerTStage<TMessage> : BuilderStageBase
	{
		private readonly InProgressMessageRegistration inProgressMessage;
		private readonly InProgressDomainRegistration inProgressDomain;
		private static readonly Type messageRuntimeType;
		private static Type[]? requestParameterTypes;
		private static readonly PropertyInfo[] messageClassProperties;

		static FluntSetupHandlerTStage()
		{
			messageRuntimeType = typeof(TMessage);
			messageClassProperties = messageRuntimeType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}

		public FluntSetupHandlerTStage(IServiceCollection services, InProgressMessageRegistration inProgressMessage, InProgressDomainRegistration inProgressDomain) : base(services)
		{
			this.inProgressMessage = inProgressMessage;
			this.inProgressDomain = inProgressDomain;
		}

		public FluntSetupHandlerTStage<TMessage> Returns(Type messageResponseRuntimeType)
		{
			return Returns(messageResponseRuntimeType.Name, messageResponseRuntimeType);
		}

		public FluntSetupHandlerTStage<TMessage> Returns(string repsonseTypeDisplayName, Type messageResponseRuntimeType)
		{
			inProgressMessage.ResponseRunTimeType = messageResponseRuntimeType;
			inProgressMessage.ResponseRunTimeTypeDisplayName = repsonseTypeDisplayName;
			return this;
		}

		public FluntSetupHandlerTStage<TMessage> Returns<TReponse>()
		{
			var responseType = typeof(TReponse);
			return Returns(responseType);
		}

		public FluntSetupHandlerTStage<TMessage> Returns<TReponse>(string repsonseTypeDisplayName)
		{
			var responseType = typeof(TReponse);
			return Returns(repsonseTypeDisplayName, responseType);
		}

		public FluentSetupDomainPostStage HandeledBy(Action<TMessage, RequestResult> handler)
		{

			Action<RequestResult> innerHandler = (result) =>
			{
				var message = CreateMessageFromRequest(result.Request);
				handler.Invoke(message, result);
			};
			inProgressMessage.RequestHandler = innerHandler;
			return new FluentSetupDomainPostStage(services, inProgressDomain);
		}

		private static TMessage CreateMessageFromRequest(Request request)
		{
			TMessage? messageInstance;

			if (TryCreateMessageWithCtor(request, out messageInstance))
			{
				return messageInstance!;
			}

			if (TryCreateMessageWithSetters(request, out messageInstance))
			{
				return messageInstance!;
			}

			throw new Exception("Failed to create instance of message");
		}

		private static bool TryCreateMessageWithCtor(Request request, out TMessage? message)
		{
			CacheParameterTypes(request);

			var promisingCtor = FluntSetupHandlerTStage<TMessage>.messageRuntimeType.GetConstructor(requestParameterTypes);

			if (promisingCtor is null)
			{
				message = default;
				return false;
			}
			var requestParameterValues = request.Parameters.Select(x => x.Value).ToArray();
			try
			{
				var messageInstance = (TMessage)promisingCtor.Invoke(requestParameterValues);
				message = messageInstance;
				return true;
			}
			catch (Exception ex)
			{
				message = default;
				return false;
			}
		}


		private static bool TryCreateMessageWithSetters(Request request, out TMessage? message)
		{
			CacheParameterTypes(request);

			if (messageClassProperties.Length != requestParameterTypes!.Length)
			{
				message = default;
				return false;
			}

			if (messageClassProperties.Select(x => x.PropertyType).SequenceEqual(requestParameterTypes))
			{
				message = default;
				return false;
			}

			TMessage messageInstance = Activator.CreateInstance<TMessage>();
			for (int parameterIndex = 0; parameterIndex < requestParameterTypes.Length; parameterIndex++)
			{
				var requestParameterType = requestParameterTypes[parameterIndex];
				var messagePropertyInfo = messageClassProperties[parameterIndex];
				var requestParameter = request.Parameters.ElementAt(parameterIndex);

				messagePropertyInfo.SetValue(messageInstance, requestParameter.Value);
			}

			message = messageInstance;
			return true;
		}

		private static void CacheParameterTypes(Request request)
		{
			if (requestParameterTypes is null)
			{
				requestParameterTypes = request.RequestInfo.Parameters.Select(x => x.Type).ToArray();
			}

		}
	}
}
