using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderHeaderDto orderForUpdate) : IRequest<int>;
}
