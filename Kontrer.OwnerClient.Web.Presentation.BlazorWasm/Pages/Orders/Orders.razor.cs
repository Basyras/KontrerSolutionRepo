using Kontrer.OwnerClient.Application.Orders;
using Kontrer.OwnerClient.Web.Presentation.BlazorWasm.Shared.Dialogs;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerClient.Web.Presentation.BlazorWasm.Pages.Orders
{
    public partial class Orders
    {
        public List<OrderViewModel> ToProcessOrders { get; set; } = new List<OrderViewModel>();
        public List<OrderViewModel> HistoryOrders { get; set; } = new List<OrderViewModel>();
        private bool isLoading = false;
        private bool loadingFailed = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Refresh();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Refresh()
        {
            loadingFailed = false;
            isLoading = true;
            this.StateHasChanged();
            List<OrderViewModel> allOrders = new List<OrderViewModel>();
            try
            {
                allOrders = await orderManager.GetOrders();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error");
                loadingFailed = true;
                isLoading = false;
                this.StateHasChanged();
                return;
            }

            ToProcessOrders.Clear();
            foreach (var order in allOrders)
            {
                switch (order.Order.State)
                {
                    case OwnerServer.OrderService.Domain.Orders.OrderStates.New:
                        ToProcessOrders.Add(order);
                        break;

                    case OwnerServer.OrderService.Domain.Orders.OrderStates.Processed:
                        ToProcessOrders.Add(order);
                        break;

                    case OwnerServer.OrderService.Domain.Orders.OrderStates.CanceledByOwner:
                        ToProcessOrders.Add(order);
                        break;
                }
            }
            isLoading = false;
            this.StateHasChanged();
        }

        public void ShowEditor()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, DisableBackdropClick= true };            
            var parameters = new DialogParameters();            
            parameters.Add(nameof(OkCancelDialog.Content), CreateEditor());
            DialogService.Show<OkCancelDialog>("test", parameters, options);            
        }

        public async Task SeedData()
        {
            await DataSeeder.SeedData(services);
        }
    }
}