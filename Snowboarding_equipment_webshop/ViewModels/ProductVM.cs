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
        public double PriceFor5To10 { get; set; }
        public double PriceForMoreThan10 { get; set; }
        public double? OldPrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ThumbnailImageId { get; set; }
        public ThumbnailImage ThumbnailImage { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [DisplayName("Thumbnail image")]
        public IFormFile NewThumbnailImage { get; set; }

        [DisplayName("Gallery images")]
        public IEnumerable<IFormFile> GalleryImages { get; set; }
    }
}
