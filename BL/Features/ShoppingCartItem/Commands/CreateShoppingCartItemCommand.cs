using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands
{
    public record CreateShoppingCartItemCommand(ShoppingCartItemDto newShoppingCartItem) : IRequest<int>;
}
