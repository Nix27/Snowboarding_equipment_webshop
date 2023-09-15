using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetAllOrders
{
    public record GetAllOrdersQuery(string? includeProperties = null) : IRequest<IEnumerable<OrderHeaderDto>>;
}
