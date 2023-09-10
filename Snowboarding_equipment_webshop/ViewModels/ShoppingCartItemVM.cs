using DAL.Models;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class ShoppingCartItemVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
