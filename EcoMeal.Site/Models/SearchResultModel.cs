using System.Collections.Generic;

namespace EcoMeal.Site.Models
{
    public class SearchResultModel
    {
        public List<BusinessModel> Businesses { get; set; } = new List<BusinessModel>();
        public List<PackageGetModel> Packages { get; set; } = new List<PackageGetModel>();
    }
}