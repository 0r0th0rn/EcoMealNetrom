using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.Pages.Orders;

public partial class Orders
{
    [Inject]
    public required OrderService OrderService { get; set; }
    private List<OrderGetModel>? MyOrders;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            MyOrders = await OrderService.GetMyOrderAsync();
            StateHasChanged();
        }
    }
}