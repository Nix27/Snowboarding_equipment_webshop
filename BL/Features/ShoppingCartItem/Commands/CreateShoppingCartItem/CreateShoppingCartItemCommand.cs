using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.CreateShoppingCartItem
{
    public record CreateShoppingCartItemCommand(ShoppingCartItemDto newShoppingCartItem) : IRequest<int>;
}
