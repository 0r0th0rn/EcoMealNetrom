using System.ComponentModel.DataAnnotations;

namespace EcoMeal.API.Models;

public class OrderCreateDTO
{
    [Required]
    public int PackageId { get; set; }
}