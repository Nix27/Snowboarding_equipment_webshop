using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemByProductIdAndUserId
{
    public record class GetShoppingCartItemByProductIdAndUserIdQuery(int productId, string userId) : IRequest<ShoppingCartItemDto?>;
}
