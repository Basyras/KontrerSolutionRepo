using Kontrer.OwnerServer.CustomerService.Domain.Customer;
using Kontrer.OwnerServer.OrderService.Domain.Orders.AccommodationOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Application.Orders
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
        }

        public OrderViewModel(CustomerEntity customer, AccommodationOrderEntity order)
        {
            Customer = customer;
            Order = order;
        }

        public CustomerEntity Customer { get; set; }
        public AccommodationOrderEntity Order { get; set; }
    }
}