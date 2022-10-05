using Basyc.MessageBus.Client.MasstTransit;
using Basyc.MicroService.Asp.Bootstrapper;
using Basyc.Repositories.EF;
using Kontrer.OwnerServer.OrderService.Application.Order.AccommodationOrder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Kontrer.OwnerServer.OrderService.Presentation.AspApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = MicroserviceBootstrapper.CreateBuilder<Startup>(args);

			builder.AddMessageBus()
				.NoProxy()
				.RegisterBasycTypedHandlers<CreateAccommodationOrderCommandHandler>()
				.UseMassTransitProvider();

			builder
				.Back()
				.MigrateDatabaseOnStart<DbContext>()
				//.MigrateDatabaseOnStart<OrderServiceDbContext>()
				.Build()
				.Run();
		}
	}
}