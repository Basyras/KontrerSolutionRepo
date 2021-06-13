using Kontrer.OwnerServer.OrderService.Client.Models;
using Kontrer.OwnerServer.OrderService.Client.Models.Blueprints;
using Kontrer.OwnerServer.OrderService.Infrastructure.Abstraction;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse.Customers;
using Kontrer.OwnerServer.Shared.MicroService.Abstraction.MessageBus.RequestResponse.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.OrderService.Application.Accommodation
{
    public class AccommodationOrderManager
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
            int orderId = orderIdResponse.Id;
            AccommodationOrder order = new AccommodationOrder(orderId, customerId, blueprint, DateTime.Now, OrderStates.Processed, orderCulture, null, null);
            order.State = OrderStates.New;
            return await orderRepository.AddAsync(order);
        }

        public async Task EditOrderAsync(int orderId, AccommodationBlueprint accommodationBlueprint)
        {
            var oldOrder = await orderRepository.TryGetAsync(orderId);
            oldOrder.Blueprint = accommodationBlueprint;
            await orderRepository.UpdateAsync(oldOrder);
        }

        public async Task ProcessOrderAsync(int orderId)
        {
            var oldOrder = await orderRepository.TryGetAsync(orderId);
            oldOrder.State = OrderStates.Processed;
            await orderRepository.UpdateAsync(oldOrder);
        }

        public async Task CompleteOrderAsync(int orderId)
        {
            var oldOrder = await orderRepository.TryGetAsync(orderId);
            oldOrder.State = OrderStates.Completed;
            await orderRepository.UpdateAsync(oldOrder);
        }

        public async Task CancelOrderAsync(int orderId, string reason, bool isCanceledByCustomer)
        {
            var oldOrder = await orderRepository.TryGetAsync(orderId);
            oldOrder.State = isCanceledByCustomer ? OrderStates.CanceledByCustomer : OrderStates.CanceledByOwner;
            await orderRepository.UpdateAsync(oldOrder);
        }

        public async Task<List<AccommodationOrder>> GetNewAsync()
        {
            var dic = await orderRepository.GetNewOrdersAsync();
            var orders = dic.Select(x => x.Value).ToList();
            return orders;
        }

        public async Task<List<AccommodationOrder>> GetProcessedAsync()
        {
            var dic = await orderRepository.GetProcessedAsync();
            var orders = dic.Select(x => x.Value).ToList();
            return orders;
        }

        public async Task<List<AccommodationOrder>> GetCompletedAsync()
        {
            var dic = await orderRepository.GetCompletedAsync();
            var orders = dic.Select(x => x.Value).ToList();
            return orders;
        }
    }
}