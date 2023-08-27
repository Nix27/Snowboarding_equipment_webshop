using System.ComponentModel.DataAnnotations;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class CountryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name of country is required")]
        public string Name { get; set; }
    }
}
