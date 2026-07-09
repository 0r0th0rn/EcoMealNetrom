using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.Pages
{
    public partial class EditBusiness
    {
        [Parameter]
        public int BusinessId { get; set; }

        [Inject]
        public required BusinessService BusinessService { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        public BusinessAddModel BusinessEditModel { get; set; } = new();
        public List<BusinessTypeModel> BusinessTypes { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            BusinessTypes = await BusinessService.GetBusinessTypesAsync();
            var existingBusiness = await BusinessService.GetBusinessForEditAsync(BusinessId);
            if (existingBusiness != null)
            {
                BusinessEditModel = existingBusiness;
            }
        }

        public async Task UpdateBusiness()
        {
            await BusinessService.UpdateBusinessAsync(BusinessId, BusinessEditModel);
            NavigationManager.NavigateTo("/");
        }

        public void GoBack()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}