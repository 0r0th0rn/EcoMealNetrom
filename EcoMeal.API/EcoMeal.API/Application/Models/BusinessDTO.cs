using System.ComponentModel.DataAnnotations.Schema;

namespace EcoMeal.API.Entities;

public class BusinessDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public string? Contact { get; set; }
    public string? BusinessTypeName { get; set; }
}