using Basyc.MessageBus.Client;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SandBox.AspApiApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DebugController : ControllerBase
	{
		private readonly ITypedMessageBusClient messageBusManager;

		public DebugController(ITypedMessageBusClient messageBusManager)
		{
			this.messageBusManager = messageBusManager;
		}

		[HttpGet]
		public async Task Get()
		{
			//await messageBusManager.SendAsync<CancelAccommodationOrderCommand>(new CancelAccommodationOrderCommand(0, "0", true));
			await messageBusManager.SendAsync<DeleteCustomerCommand>(new DeleteCustomerCommand(1)).Task;
		}
	}
}