using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DeleteShoppingCartItem
{
    public record DeleteShoppingCartItemCommand(int shoppingCartItemId) : IRequest<int>;
}
