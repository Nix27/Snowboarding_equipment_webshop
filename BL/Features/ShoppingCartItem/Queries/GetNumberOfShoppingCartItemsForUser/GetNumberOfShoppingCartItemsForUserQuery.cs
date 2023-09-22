using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetNumberOfShoppingCartItemsForUser
{
    public record GetNumberOfShoppingCartItemsForUserQuery(string userId) : IRequest<int>;
}
