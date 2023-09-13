using BL.DTOs;

namespace Snowboarding_equipment_webshop.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCartItemDto> ShoppingCartItems { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
    }
}
