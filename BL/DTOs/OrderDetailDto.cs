using DAL.Models;

namespace BL.DTOs
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
