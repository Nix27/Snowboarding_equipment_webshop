using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DeleteMultipleShoppingCartItems
{
    public record DeleteMultipleShoppingCartItemsCommand(IEnumerable<ShoppingCartItemDto> shoppingCartItemsForDelete) : IRequest;
}
