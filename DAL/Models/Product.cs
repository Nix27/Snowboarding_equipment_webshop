using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityInStock { get; set; }
        public double Price { get; set; }
        public double PriceFor5To10 { get; set; }
        public double PriceForMoreThan10 { get; set; }
        public double? OldPrice { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int ThumbnailImageId { get; set; }
        [ForeignKey(nameof(ThumbnailImageId))]
        public ThumbnailImage ThumbnailImage { get; set; }

        public ICollection<GalleryImage> GalleryImages { get; set; }
    }
}
