namespace EcoMeal.API.Models;

public class PackageGetDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime StartPickup { get; set; }
    public DateTime EndPickup { get; set; }
    public required string PackageTypeName { get; set; }
}
