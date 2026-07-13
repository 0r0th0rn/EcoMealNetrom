using System.ComponentModel.DataAnnotations;

namespace EcoMeal.API.Entities;

public class Package
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public required DateTime StartRidicare { get; set; }
    public required DateTime EndRidicare { get; set; }
    public int BusinessId { get; set; }
    public Business? Business { get; set; }
    public int PackageTypeId { get; set; }
    public PackageType? PackageType { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}