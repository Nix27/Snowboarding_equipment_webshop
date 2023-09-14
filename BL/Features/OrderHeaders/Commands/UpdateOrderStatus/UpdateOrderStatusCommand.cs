using MediatR;

namespace BL.Features.OrderHeaders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int orderHeaderId, string orderStatus, string? paymentStatus = null) : IRequest;
}
