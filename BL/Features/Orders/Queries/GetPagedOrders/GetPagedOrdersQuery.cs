using BL.DTOs;
using MediatR;

namespace BL.Features.Orders.Queries.GetPagedOrders
{
    public record GetPagedOrdersQuery(IEnumerable<OrderDto>? orders, int size, int page) : IRequest<IEnumerable<OrderDto>>;
}
