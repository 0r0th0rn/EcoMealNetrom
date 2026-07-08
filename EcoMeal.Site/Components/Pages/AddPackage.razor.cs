using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.Pages
{
    public partial class AddPackage
    {
        [Parameter]
        public int BusinessId { get; set; }
        [Inject]
        public required BusinessService BusinessService { get; set; }
        public PackageAddModel PackageAddModel { get; set; } = new PackageAddModel()
        {
            Name = string.Empty,
            Description = string.Empty,
            Price = 0,
            StartPickup = DateTime.Now,
            EndPickup = DateTime.Now.AddHours(2),
            PackageTypeId = 0
        };

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        public List<PackageTypeModel> PackageTypes { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            PackageTypes = await BusinessService.GetPackageTypesAsync();
        }

        public async Task AddPackageToBusiness()
        {
            await BusinessService.AddPackageToBusiness(BusinessId, PackageAddModel);
            NavigationManager.NavigateTo($"/business/{BusinessId}");
        }

        public void GoBack()
        {
            NavigationManager.NavigateTo($"/business/{BusinessId}");
        }
    }
}