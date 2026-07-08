using EcoMeal.API.Entities;

namespace EcoMeal.API.Models;

public class BusinessDetailsDTO : BusinessDTO
{
    public List<PackageGetDTO> Packages { get; set; } = new();
}