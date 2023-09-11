using BL.DTOs;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemByFilter
{
    public record class GetShoppingCartItemByFilterQuery(Expression<Func<DAL.Models.ShoppingCartItem, bool>> filter) : IRequest<ShoppingCartItemDto?>;
}
