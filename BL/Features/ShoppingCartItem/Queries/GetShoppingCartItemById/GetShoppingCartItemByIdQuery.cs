using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemById
{
    public record GetShoppingCartItemByIdQuery(int shoppingCartItemId, bool isTracked = true) : IRequest<ShoppingCartItemDto?>;
}
