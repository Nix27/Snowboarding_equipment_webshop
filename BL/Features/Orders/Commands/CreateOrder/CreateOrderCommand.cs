using BL.DTOs;
using MediatR;

namespace BL.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDto newOrder) : IRequest<int>;
}
