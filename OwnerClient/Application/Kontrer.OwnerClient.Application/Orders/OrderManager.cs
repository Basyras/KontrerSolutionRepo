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
    public class OrderManager : IOrderManager
    {
        private readonly IMessageBusManager bus;
        private List<OrderViewModel> ordersCache = new List<OrderViewModel>();
        private DateTime lastRefresh;

        public OrderManager(IMessageBusManager bus)
        {
            this.bus = bus;
        }

        public async ValueTask<List<OrderViewModel>> GetNewOrders()
        {
            if (NeedsRefresh())
            {
                var response = await bus.RequestAsync<GetNewAccommodationOrdersQuery, GetNewAccommodationOrdersResponse>();
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

            return ordersCache;
        }

        private bool NeedsRefresh()
        {
            return lastRefresh.AddSeconds(5) < DateTime.Now;
        }
    }
}