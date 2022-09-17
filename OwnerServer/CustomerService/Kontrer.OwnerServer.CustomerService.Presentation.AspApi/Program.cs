using Basyc.MicroService.Asp.Bootstrapper;
using Kontrer.OwnerServer.CustomerService.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.CustomerService.Presentation.AspApi
{
	public class Program
	{
		private const string debugConnectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerServiceDB;Trusted_Connection=True;MultipleActiveResultSets=true";

		public static async Task Main(string[] args)
		{
			var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);

			builder.services.AddBasycMessageBus()
				.NoProxy()
				.RegisterBasycTypedHandlers<CustomerServiceApplicationAssemblyMarker>()
				.UseNetMQProvider("CustomerService")
				.NoDiagnostics();

			new CustomerInfrastructureBuilder(builder.services)
				.AddEFRespository()
				.AddSqlServer(debugConnectionString);

			await builder.Back().Build().RunAsync();
		}
	}
}