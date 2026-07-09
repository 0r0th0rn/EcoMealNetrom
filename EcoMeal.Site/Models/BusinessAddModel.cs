using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Site.Models
{
    public class BusinessAddModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adresa este obligatorie.")]
        public string Address { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contactul este obligatoriu.")]
        public string Contact { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Alege un tip de afacere.")]
        public int BusinessTypeId { get; set; }
    }
}