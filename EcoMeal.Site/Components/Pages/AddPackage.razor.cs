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
            PackageTypeId = 1
        };

        public async Task AddPackageToBusiness()
        {
            await BusinessService.AddPackageToBusiness(BusinessId, PackageAddModel);
        }
    }
}