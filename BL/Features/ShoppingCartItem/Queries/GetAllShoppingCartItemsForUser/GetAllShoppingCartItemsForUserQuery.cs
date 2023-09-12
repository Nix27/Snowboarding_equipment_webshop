using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetAllShoppingCartItemsForUser
{
    public record GetAllShoppingCartItemsForUserQuery(string userId) : IRequest<IEnumerable<ShoppingCartItemDto>>;
}
