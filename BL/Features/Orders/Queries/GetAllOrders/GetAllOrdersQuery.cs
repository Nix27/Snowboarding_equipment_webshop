using BL.DTOs;
using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Orders.Queries.GetAllOrders
{
    public record GetAllOrdersQuery(Expression<Func<Order, bool>>? filter = null,
        string? includeProperties = null,
        bool isTracked = true) : IRequest<IEnumerable<OrderDto>>;
}
