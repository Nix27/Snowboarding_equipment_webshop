using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime DateOfOrder { get; set; }
        public DateTime DateOfShipping { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CompanyPaymentDeadline { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public double TotalPrice { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
