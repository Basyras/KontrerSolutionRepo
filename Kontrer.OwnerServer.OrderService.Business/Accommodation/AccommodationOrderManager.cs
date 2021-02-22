using Kontrer.OwnerServer.OrderService.Business.Abstraction.Accommodation;
using Kontrer.OwnerServer.OrderService.Data.Abstraction;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests.Customers;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests.Orders;
using Kontrer.Shared.Models;
using Kontrer.Shared.Models.Pricing.Blueprints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Business.Accommodation
{
    public class AccommodationOrderManager : IAccommodationOrderManager
    {
       
        private readonly IAccommodaionOrderRepository orderRepository;

        public AccommodationOrderManager(IAccommodaionOrderRepository orderRepository,IMessageBusManager messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        private readonly IMessageBusManager messageBus;  


        public async Task<AccommodationOrder> CreateOrder(int customerId, AccommodationBlueprint blueprint, CultureInfo orderCulture)
        {
            int orderId = await messageBus.RequestAsync<CreateOrderIdRequest,int>();
            var order = new AccommodationOrder(orderId, customerId, blueprint, DateTime.Now, OrderStates.WaitingForCustomerResponse, orderCulture, null, null);
            orderRepository.AddOrder(order);
            await orderRepository.CommitAsync();
            return order;
        }

        public void CancelOrder(int orderId, string reason, bool isCanceledByCustomer)
        {
            throw new NotImplementedException();
        }

        public void EditOrder()
        {
            throw new NotImplementedException();
        }

        public Task<List<AccommodationOrder>> GetOrders()
        {
            throw new NotImplementedException();
        }
    }
}
