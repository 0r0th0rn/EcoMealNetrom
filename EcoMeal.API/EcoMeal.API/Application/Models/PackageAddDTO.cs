namespace EcoMeal.API.Models;

public class PackageAddDTO
{
    public required string Name;
    public required string Description;
    public decimal Price { get; set; }
    public DateTime StartPickup { get; set; }
    public DateTime EndPickup { get; set; }
    public int PackageTypeId { get; set; }

}