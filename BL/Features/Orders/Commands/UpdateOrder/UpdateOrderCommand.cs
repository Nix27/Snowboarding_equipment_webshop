using BL.DTOs;
using MediatR;

namespace BL.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto orderForUpdate) : IRequest<int>;
}
