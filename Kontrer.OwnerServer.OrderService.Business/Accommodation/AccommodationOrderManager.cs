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
        public AccommodationOrderManager(IAccommodaionOrderRepository orderRepository, IMessageBusManager messageBus)
        {
            this.orderRepository = orderRepository;
            this.messageBus = messageBus;
        }

        private readonly IMessageBusManager messageBus;

        public async Task<AccommodationOrder> CreateOrderAsync(int customerId, AccommodationBlueprint blueprint, CultureInfo orderCulture)
        {

            CreateOrderIdResponse orderIdResponse = await messageBus.RequestAsync<CreateOrderIdRequest, CreateOrderIdResponse>();
            int orderId = orderIdResponse.Data;
            AccommodationOrder order = new AccommodationOrder(orderId, customerId, blueprint, DateTime.Now, OrderStates.WaitingForCustomerResponse, orderCulture, null, null);
            orderRepository.AddAsync(order);
            await orderRepository.CommitAsync();
            return order;

        }

        public async Task CancelOrderAsync(int orderId, string reason, bool isCanceledByCustomer)
        {
            var oldOrder = await orderRepository.TryGetAsync(orderId);
            oldOrder.State = isCanceledByCustomer ? OrderStates.CanceledByCustomer : OrderStates.CanceledByOwner;
            orderRepository.UpdateAsync(oldOrder);
            await orderRepository.CommitAsync();            
        }

        public async Task EditOrderAsync(int orderId, AccommodationBlueprint accommodationBlueprint)
        {
            var oldOrder = await orderRepository.TryGetAsync(orderId);
            oldOrder.Blueprint = accommodationBlueprint;
            orderRepository.UpdateAsync(oldOrder);
            await orderRepository.CommitAsync();

        }

        public async Task<List<AccommodationOrder>> GetOrders()
        {
            var dic = await orderRepository.TryGetAsync();
            var orders = dic.Select(x => x.Value).ToList();
            return orders;
            
        }
    }
}
