using Kontrer.OwnerClient.Application.Customers;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Orders
{
    //Exists only for designing ui
    public class MockOrderManager : IOrderManager
    {
        private readonly ICustomerManager customerManager;
        private List<OrderViewModel> orders = new List<OrderViewModel>();

        public MockOrderManager(ICustomerManager customerManager)
        {
            this.customerManager = customerManager;
        }

        public async ValueTask<OrderViewModel> CreateOrder(int customerId)
        {
            var newOrder = new OrderViewModel();
            newOrder.Customer = (await customerManager.GetCustomers(new int[] { customerId })).First();
            newOrder.Order = new AccommodationOrderEntity() { Id = new Random().Next(), CustomerId = customerId, State = OwnerServer.OrderService.Domain.Orders.OrderStates.New };
            orders.Add(newOrder);
            return newOrder;
        }

        public Task DeleteOrder(int orderId)
        {
            orders.Remove(orders.First(x => x.Order.Id == orderId));
            return Task.CompletedTask;
        }

        public ValueTask<List<OrderViewModel>> GetOrders()
        {
            return ValueTask.FromResult(orders);
        }

        public Task Process(AccommodationOrderEntity order)
        {
            order.State = OwnerServer.OrderService.Domain.Orders.OrderStates.Processed;
            return Task.CompletedTask;
        }
    }
}