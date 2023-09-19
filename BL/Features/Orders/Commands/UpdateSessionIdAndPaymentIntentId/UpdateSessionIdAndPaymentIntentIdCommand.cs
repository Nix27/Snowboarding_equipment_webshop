using MediatR;

namespace BL.Features.Orders.Commands.UpdateSessionIdAndPaymentIntentId
{
    public record UpdateSessionIdAndPaymentIntentIdCommand(int orderId, string sessionId, string paymentIntentId) : IRequest;
}
