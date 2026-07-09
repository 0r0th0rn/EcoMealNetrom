using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.Pages;

public partial class EditPackage
{
    [Parameter]
    public int BusinessId { get; set; }

    [Parameter]
    public int PackageId { get; set; }

    [Inject]
    public required BusinessService BusinessService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    public PackageAddModel PackageEditModel { get; set; } = new()
    {
        Name = string.Empty,
        Description = string.Empty,
        Price = 0,
        StartPickup = DateTime.Now,
        EndPickup = DateTime.Now.AddHours(2),
        PackageTypeId = 0
    };

    public List<PackageTypeModel> PackageTypes { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        PackageTypes = await BusinessService.GetPackageTypesAsync();

        var existingPackage = await BusinessService.GetPackageForEditAsync(PackageId);
        if (existingPackage != null)
        {
            PackageEditModel = existingPackage;
        }
    }

    public async Task UpdatePackage()
    {
        await BusinessService.UpdatePackageAsync(PackageId, PackageEditModel);
        NavigationManager.NavigateTo($"/business/{BusinessId}");
    }

    public void GoBack()
    {
        NavigationManager.NavigateTo($"/business/{BusinessId}");
    }
}