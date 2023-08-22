using System.ComponentModel.DataAnnotations;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
