using EcoMeal.API.Entities;
using EcoMeal.API.Models;

namespace EcoMeal.API.Application.Models;

public class SearchResultDTO
{
    public List<BusinessDTO> Businesses { get; set; } = new List<BusinessDTO>();
    public List<PackageGetDTO> Packages { get; set; } = new List<PackageGetDTO>();
}