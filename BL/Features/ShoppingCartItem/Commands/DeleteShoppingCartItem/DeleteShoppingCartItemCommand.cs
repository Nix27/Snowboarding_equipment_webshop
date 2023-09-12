using BL.DTOs;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DeleteShoppingCartItem
{
    public record DeleteShoppingCartItemCommand(ShoppingCartItemDto ShoppingCartItemForDelete) : IRequest;
}
