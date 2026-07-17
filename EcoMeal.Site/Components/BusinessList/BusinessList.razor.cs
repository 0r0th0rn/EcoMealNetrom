using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EcoMeal.Site.Components.BusinessList;

public partial class BusinessList
{
    [Inject]
    public required BusinessService BusinessService { get; set; }
    
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private List<BusinessModel>? Businesses { get; set; }
    private bool isSorting = false;

    protected override async Task OnInitializedAsync()
    {
        Businesses = await BusinessService.GetAllSync();
    }

    private async Task HandleDelete(int businessId)
    {
        bool success = await BusinessService.DeleteAsync(businessId);

        if (success)
        {
            Businesses?.RemoveAll(b => b.Id == businessId);
        }
    }

    private async Task SortByDistance()
    {
        isSorting = true;
        StateHasChanged();
        
        try
        {
            var location = await JSRuntime.InvokeAsync<LocationData>("leafletInterop.getUserLocation");
            Businesses = await BusinessService.GetAllSync(location.Latitude, location.Longitude);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare geolocalizare: {ex.Message}");
        }
        finally
        {
            isSorting = false;
            StateHasChanged();
        }
    }

    public class LocationData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}