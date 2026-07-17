namespace EcoMeal.Site.Models;

public class BusinessModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? Description { get; set; }
    public required string Contact { get; set; }
    public required string BusinessTypeName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? DistanceInKm { get; set; }
}
