using BL.DTOs;
using MediatR;

namespace BL.Features.OrderHeaders.Commands.CreateOrderHeader
{
    public record CreateOrderHeaderCommand(OrderHeaderDto newOrderHeader) : IRequest<int>;
}
