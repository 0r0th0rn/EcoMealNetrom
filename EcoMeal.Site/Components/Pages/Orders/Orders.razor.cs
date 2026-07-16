using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EcoMeal.Site.Components.Pages.Orders;

public partial class Orders
{
    private bool IsLoading = true;
    [Inject]
    public OrderService OrderService { get; set; } = default!;

    [Inject]
    public AuthService AuthService { get; set; } = default!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    private List<OrderGetModel> AllOrders = new();
    private bool IsAdmin = false;

    private IEnumerable<OrderGetModel> OngoingOrders => AllOrders.Where(o => o.Status != "Completata");
    private IEnumerable<OrderGetModel> CompletedOrders => AllOrders.Where(o => o.Status == "Completata");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Determine if the user is admin
            var authState = await AuthenticationStateTask;
            IsAdmin = authState.User.IsInRole("Admin");

            await LoadOrdersAsync();
            StateHasChanged();
        }
    }

    private async Task LoadOrdersAsync()
    {
        IsLoading = true;
        try
        {
            if (IsAdmin)
            {
                AllOrders = await OrderService.GetAllOrdersAsync();
            }
            else
            {
                AllOrders = await OrderService.GetMyOrderAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la încărcarea comenzilor: {ex.Message}");
            AllOrders = new();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task MarkAsCompleted(int orderId)
    {
        var success = await OrderService.CompleteOrderAsync(orderId);

        if (success)
        {
            var order = AllOrders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = "Completata";
            }
            StateHasChanged();
        }
        else
        {
            Console.WriteLine("Eroare la actualizarea statusului.");
        }
    }
}