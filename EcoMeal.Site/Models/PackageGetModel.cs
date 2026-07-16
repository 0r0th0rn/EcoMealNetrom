namespace EcoMeal.Site.Models;

public class PackageGetModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public double weightInKg { get; set; }
    public int AvailableQty { get; set; }
    public DateTime StartPickup { get; set; }
    public DateTime EndPickup { get; set; }
    public string PackageTypeName { get; set; } = "";
    public int BusinessId { get; set; }

}
