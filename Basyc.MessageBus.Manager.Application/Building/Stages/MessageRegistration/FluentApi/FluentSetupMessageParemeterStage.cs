using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using ParameterInfo = Basyc.MessageBus.Manager.Application.Initialization.ParameterInfo;

namespace Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration.FluentApi
{
	public class FluentSetupMessageParemeterStage : BuilderStageBase
	{
		private readonly InProgressMessageRegistration inProgressMessage;
		private readonly InProgressDomainRegistration inProgressDomain;

		public FluentSetupMessageParemeterStage(IServiceCollection services, InProgressMessageRegistration inProgressMessage, InProgressDomainRegistration inProgressDomain) : base(services)
		{
			this.inProgressMessage = inProgressMessage;
			this.inProgressDomain = inProgressDomain;
		}

		public FluentSetupMessageParemeterStage WithParameter<TParameter>(string parameterDisplayName)
		{
			inProgressMessage.Parameters.Add(new ParameterInfo(typeof(TParameter), parameterDisplayName, typeof(TParameter).Name));
			return new FluentSetupMessageParemeterStage(services, inProgressMessage, inProgressDomain);
		}

		public FluntSetupHandlerTStage<TMessage> WithParameters<TMessage>()
		{
			foreach (var parameter in typeof(TMessage).GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				inProgressMessage.Parameters.Add(new ParameterInfo(parameter.PropertyType, parameter.Name, parameter.PropertyType.Name));
			}
			return new FluntSetupHandlerTStage<TMessage>(services, inProgressMessage, inProgressDomain);
		}

		public FluentSetupMessageParemeterStage Returns(Type messageResponseRuntimeType)
		{
			return Returns(messageResponseRuntimeType.Name, messageResponseRuntimeType);
		}

		public FluentSetupMessageParemeterStage Returns(string repsonseTypeDisplayName, Type messageResponseRuntimeType)
		{
			inProgressMessage.ResponseRunTimeType = messageResponseRuntimeType;
			inProgressMessage.ResponseRunTimeTypeDisplayName = repsonseTypeDisplayName;
			return this;
		}

		public FluentSetupMessageParemeterStage Returns<TReponse>()
		{
			var responseType = typeof(TReponse);
			return Returns(responseType);
		}

		public FluentSetupMessageParemeterStage Returns<TReponse>(string repsonseTypeDisplayName)
		{
			var responseType = typeof(TReponse);
			return Returns(repsonseTypeDisplayName, responseType);
		}

		public FluentSetupDomainPostStage HandeledBy(Action<RequestResult> handler)
		{
			inProgressMessage.RequestHandler = handler;
			return new FluentSetupDomainPostStage(services, inProgressDomain);
		}
	}
}
