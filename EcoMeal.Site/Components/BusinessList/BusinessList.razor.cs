using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.BusinessList;

public partial class BusinessList
{
    [Inject]
    public required BusinessService BusinessService { get; set; }
    private List<BusinessModel>? Businesses { get; set; }
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
}