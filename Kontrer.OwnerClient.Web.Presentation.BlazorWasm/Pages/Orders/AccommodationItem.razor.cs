using Kontrer.OwnerClient.Application.Orders;
using Kontrer.OwnerServer.OrderService.Domain.Orders;
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
        private string modifier { get => Order.Order.State == OrderStates.CanceledByOwner || Order.Order.State == OrderStates.CanceledByCustomer ? "Canceled" : Order.Order.State.ToString(); }
        private string statusLabelModifierClass { get => "statusLabel--" + modifier; }
        private string statusRibbonModifierClass { get => "statusRibbon--" + modifier; }
        private string statusText
        {
            get
            {
                return Order.Order.State switch
                {
                    OrderStates.New => "to process",
                    OrderStates.Processed => DateTime.Now > Order.Order.Requirment.From ? DateTime.Now < Order.Order.Requirment.To ? "hapenning" : "happened" : "to happen",
                    OrderStates.Completed => "happened",
                    OrderStates.CanceledByCustomer => "canceled",
                    OrderStates.CanceledByOwner => "canceled",
                    _ => throw new NotImplementedException(),
                };
            }
        }
    }
}