using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetOrderHeaderById
{
    public record GetOrderHeaderByIdQuery(int orderHeaderId, string? includeProperties = null) : IRequest<OrderHeaderDto?>;
}
