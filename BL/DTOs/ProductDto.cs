using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int QuantityInStock { get; set; }
        public double Price { get; set; }
        public double PriceFor5To10 { get; set; }
        public double PriceForMoreThan10 { get; set; }
        public double? OldPrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ThumbnailImageId { get; set; }
        public ThumbnailImage ThumbnailImage { get; set; }
        public ICollection<GalleryImage> GalleryImages { get; set; }
    }
}
