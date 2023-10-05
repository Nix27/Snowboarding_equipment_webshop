using BL.DTOs;
using MediatR;

namespace BL.Features.Orders.Queries.GetPagedOrders
{
    public record GetPagedOrdersQuery(float size, int page, string filterBy) : IRequest<IEnumerable<OrderDto>>;
}
