using Kontrer.OwnerClient.Application.Utilities.Cache;
using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using Kontrer.Shared.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Orders
{
    public class BusOrderManager : IOrderManager
    {
        private readonly IMessageBusManager bus;
        private List<OrderViewModel> ordersCache = new List<OrderViewModel>();
        private DateTime lastRefresh;
        private bool shouldRefresh;

        public BusOrderManager(IMessageBusManager bus)
        {
            this.bus = bus;
            shouldRefresh = true;
        }

        public async ValueTask<OrderViewModel> CreateOrder(int customerId)
        {
            var response = await bus.RequestAsync<CreateAccommodationOrderCommand, CreateAccommodationOrderResponse>(new(customerId, new()));
            var customerResponse = await bus.RequestAsync<GetCustomersQuery, GetCustomersQueryResponse>(new(new int[] { customerId }));
            var vm = new OrderViewModel(customerResponse.Customers[0], response.NewOrder);
            shouldRefresh = true;
            return vm;
        }

        public async Task DeleteOrder(int orderId)
        {
            await bus.SendAsync(new DeleteAccommodationOrderCommand(orderId));
            shouldRefresh = true;
        }

        public async Task Process(AccommodationOrderEntity order)
        {
            await bus.SendAsync<ProcessAccommodationOrderCommand>(new(order));
            shouldRefresh = true;
        }

        public async ValueTask<List<OrderViewModel>> GetOrders()
        {
            if (NeedsRefresh())
            {
                var response = await bus.RequestAsync<GetAccommodationOrdersQuery, GetAccommodationOrdersResponse>();
                var customerIds = response.NewOrders.Values.Select(x => x.CustomerId).ToArray();
                var customers = await bus.RequestAsync<GetCustomersQuery, GetCustomersQueryResponse>(new GetCustomersQuery(customerIds));
                ordersCache.Clear();
                foreach (var order in response.NewOrders.Values)
                {
                    var customer = customers.Customers.FirstOrDefault(x => x.Id == order.CustomerId);
                    customer = customer ?? new CustomerEntity();
                    ordersCache.Add(new OrderViewModel(customer, order));
                }
                lastRefresh = DateTime.Now;
            }
            shouldRefresh = false;
            return ordersCache;
        }

        private bool NeedsRefresh()
        {
            return lastRefresh.AddSeconds(2) < DateTime.Now || shouldRefresh;
        }
    }
}