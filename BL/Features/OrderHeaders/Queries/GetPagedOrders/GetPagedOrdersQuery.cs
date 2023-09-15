using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetPagedOrders
{
    public record GetPagedOrdersQuery(int size, int page, string filterBy) : IRequest<IEnumerable<OrderHeaderDto>>;
}
