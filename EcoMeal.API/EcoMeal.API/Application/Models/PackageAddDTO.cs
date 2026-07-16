namespace EcoMeal.API.Models;

public class PackageAddDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public double weightInKg { get; set; }
    public int AvailableQty { get; set; }
    public DateTime StartPickup { get; set; }
    public DateTime EndPickup { get; set; }
    public int PackageTypeId { get; set; }

}