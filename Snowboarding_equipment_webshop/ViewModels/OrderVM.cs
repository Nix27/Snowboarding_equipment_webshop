using DAL.Models;
using System.ComponentModel;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime DateOfOrder { get; set; }
        public DateTime DateOfShipping { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CompanyPaymentDeadline { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }

        [DisplayName("Tracking Number")]
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public double TotalPrice { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }

        [DisplayName("Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public ICollection<OrderDetail> OrderItems { get; set; }
    }
}
