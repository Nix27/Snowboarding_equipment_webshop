using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name of product is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DisplayName("Quantity in stock")]
        public int QuantityInStock { get; set; }
        public double Price { get; set; }

        [DisplayName("Price for 5 To 10")]
        public double PriceFor5To10 { get; set; }

        [DisplayName("Price for more than 10")]
        public double PriceForMoreThan10 { get; set; }

        [DisplayName("Old price")]
        public double? OldPrice { get; set; }

        [Required(ErrorMessage = "Category of product is required")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        public int? ThumbnailImageId { get; set; }

        [ValidateNever]
        public ThumbnailImage? ThumbnailImage { get; set; }

        [ValidateNever]
        public ICollection<GalleryImage> GalleryImages { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [ValidateNever]
        [DisplayName("Thumbnail image")]
        public IFormFile? NewThumbnailImage { get; set; }

        [ValidateNever]
        [DisplayName("Gallery images")]
        public IEnumerable<IFormFile>? NewGalleryImages { get; set; }
    }
}
