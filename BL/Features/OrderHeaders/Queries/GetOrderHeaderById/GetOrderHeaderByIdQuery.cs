using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetOrderHeaderById
{
    public record GetOrderHeaderByIdQuery(int orderHeaderId, bool isTracked = true, string? includeProperties = null) : IRequest<OrderHeaderDto?>;
}
