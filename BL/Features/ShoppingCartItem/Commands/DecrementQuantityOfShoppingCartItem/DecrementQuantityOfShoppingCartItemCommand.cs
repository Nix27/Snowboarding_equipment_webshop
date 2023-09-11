using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DecrementQuantityOfShoppingCartItem
{
    public record DecrementQuantityOfShoppingCartItemCommand(int shoppingCartItemId, int quantity) : IRequest;
}
