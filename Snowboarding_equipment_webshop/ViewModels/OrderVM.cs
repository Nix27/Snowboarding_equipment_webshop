using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [DisplayName("Date of order")]
        public DateTime DateOfOrder { get; set; }

        [DisplayName("Date of shipping")]
        public DateTime DateOfShipping { get; set; }

        [DisplayName("Payment date")]
        public DateTime PaymentDate { get; set; }

        [DisplayName("Deadline for company payment")]
        public DateTime CompanyPaymentDeadline { get; set; }
        public string? OrderStatus { get; set; }

        [DisplayName("Payment status")]
        public string? PaymentStatus { get; set; }

        [DisplayName("Tracking Number")]
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public double TotalPrice { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        [DisplayName("Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        public ICollection<OrderDetail> OrderItems { get; set; }
    }
}
