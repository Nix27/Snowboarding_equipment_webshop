using BL.DTOs;
using MediatR;

namespace BL.Features.OrderItems.Commands.CreateOrderItem
{
    public record CreateOrderItemCommand(OrderItemDto newOrderItem) : IRequest<int>;
}
