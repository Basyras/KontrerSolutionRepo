using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration;
using Basyc.MessageBus.Manager.Application.Requesting;
using Basyc.MessageBus.Manager.Application.ResultDiagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.MessageBus.Manager
{
	public static class MessageManagerIServiceCollectionExtensions
	{
		public static BusManagerApplicationBuilder AddMessageManager(this IServiceCollection services)
		{
			services.AddSingleton<IRequestManager, RequestManager>();
			services.AddSingleton<IResultLoggingManager, ResultLoggingManager>();
			var builder = new BusManagerApplicationBuilder(services);
			return builder;
		}
	}
}