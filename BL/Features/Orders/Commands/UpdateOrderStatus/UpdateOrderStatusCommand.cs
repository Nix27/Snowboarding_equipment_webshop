using MediatR;

namespace BL.Features.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int orderId, string orderStatus, string? paymentStatus = null) : IRequest;
}
