using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.Pages
{
    public partial class AddBusiness
    {
        [Inject]
        public required BusinessService BusinessService { get; set; }
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        public BusinessAddModel BusinessAddModel { get; set; } = new();
        public List<BusinessTypeModel> BusinessTypes { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            BusinessTypes = await BusinessService.GetBusinessTypesAsync();
        }
        public async Task SaveBusiness()
        {
            await BusinessService.AddBusinessAsync(BusinessAddModel);
            NavigationManager.NavigateTo("/");
        }

        public void HandleCoordinatesChanged((double lat, double lng) coords)
        {
            BusinessAddModel.Latitude = coords.lat;
            BusinessAddModel.Longitude = coords.lng;
        }

        public void GoBack()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}