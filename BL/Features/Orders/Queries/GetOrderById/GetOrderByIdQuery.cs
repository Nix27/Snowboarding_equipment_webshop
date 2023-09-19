using BL.DTOs;
using MediatR;

namespace BL.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(int orderId, bool isTracked = true, string? includeProperties = null) : IRequest<OrderDto>;
}
