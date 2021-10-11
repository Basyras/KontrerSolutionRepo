using Kontrer.OwnerClient.Application.Orders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm.Pages.Orders
{
    public partial class AccommodationItem
    {
        [Parameter]
        public OrderViewModel Order { get; set; }

        private string ribbonColor { get => Order.Order.State == OwnerServer.OrderService.Domain.Orders.OrderStates.New ? "#FFF29E" : "#FFF29E"; }
    }
}