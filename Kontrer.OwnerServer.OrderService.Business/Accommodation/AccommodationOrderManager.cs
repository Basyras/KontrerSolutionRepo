using Kontrer.OwnerServer.OrderService.Business.Abstraction.Accommodation;
using Kontrer.OwnerServer.OrderService.Data.Abstraction;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.Requests.Customers;
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


        /// <summary>
        /// Check if customer exists if not creates a new one
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="blueprint"></param>
        /// <param name="orderCulture"></param>
        /// <param name="customerNotes"></param>
        public async Task<AccommodationOrder> CreateOrder(CustomerModel customer, AccommodationBlueprint blueprint, CultureInfo orderCulture,string customerNotes = null)
        {   
            var order = await CreateOrder(customer.CustomerId,blueprint,orderCulture,customerNotes);            
            return order;
        }


        public async Task<AccommodationOrder> CreateOrder(int customerId, AccommodationBlueprint blueprint, CultureInfo orderCulture, string customerNotes = null)
        {            
            int orderId = Guid.NewGuid();
            var order = new AccommodationOrder(orderId, customerId, blueprint, DateTime.Now, OrderStates.WaitingForCustomerResponse, orderCulture, customerNotes, null, null);
            orderRepository.AddOrder(order);
            await orderRepository.CommitAsync();
            return order;

        }




    }
}
