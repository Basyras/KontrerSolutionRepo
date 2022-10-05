using Basyc.MessageBus.Client.MasstTransit;
using Basyc.MicroService.Asp.Bootstrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SandBox.AspApiApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//CreateHostBuilder(args).Build().Run();
			var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);
			builder.AddMessageBus()
					.NoProxy()
					.NoHandlers()
					.UseMassTransitProvider();

			var originalBuilder = builder.Back();
			var app = originalBuilder.Build();
			app.Run();
		}

		//public static IHostBuilder CreateHostBuilder(string[] args) =>
		//    Host.CreateDefaultBuilder(args)
		//        .ConfigureWebHostDefaults(webBuilder =>
		//        {
		//            webBuilder.ConfigureMessageBusServices<Startup>(Assembly.GetEntryAssembly());
		//            webBuilder.UseStartup<Startup>();
		//        });
	}
}