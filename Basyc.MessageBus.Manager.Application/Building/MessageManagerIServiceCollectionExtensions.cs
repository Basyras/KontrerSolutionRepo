using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager
{
	public static class MessageManagerIServiceCollectionExtensions
	{
		public static SelectMessageRegistrationMethodStage AddMessageManager(this IServiceCollection services)
		{
			services.AddSingleton<IRequestManager, RequestManager>();
			var builder = new SelectMessageRegistrationMethodStage(services);
			return builder;
		}
	}
}