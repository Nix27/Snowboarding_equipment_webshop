using MediatR;

namespace BL.Features.OrderHeaders.Commands.UpdateSessionIdAndPaymentIntentId
{
    public record UpdateSessionIdAndPaymentIntentIdCommand(int orderHeaderId, string sessionId, string paymentIntentId) : IRequest;
}
