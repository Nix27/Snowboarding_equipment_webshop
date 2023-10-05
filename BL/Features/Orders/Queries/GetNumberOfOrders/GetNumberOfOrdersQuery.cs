using DAL.Models;
using MediatR;
using System.Linq.Expressions;

namespace BL.Features.Orders.Queries.GetNumberOfOrders
{
    public record GetNumberOfOrdersQuery(Expression<Func<Order, bool>>? filter = null) : IRequest<int>;
}
