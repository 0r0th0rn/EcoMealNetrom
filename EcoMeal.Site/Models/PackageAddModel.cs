using System.ComponentModel.DataAnnotations;

using System.Globalization;

namespace EcoMeal.Site.Models;

public class PackageAddModel
{
    [Required(ErrorMessage = "Numele este obligatoriu")]
    [StringLength(50)]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Descrierea este obligatorie")]
    public required string Description { get; set; }
    [Required]
    [Range(0, 1000)]
    public double Price { get; set; }
    public double OldPrice { get; set; }
    [Required]
    public double weightInKg { get; set; }
    [Required]
    public int AvailableQty { get; set; }
    [Required]
    public DateTime StartPickup { get; set; }
    [Required]
    public DateTime EndPickup { get; set; }
    [Required]
    public int PackageTypeId { get; set; }

}