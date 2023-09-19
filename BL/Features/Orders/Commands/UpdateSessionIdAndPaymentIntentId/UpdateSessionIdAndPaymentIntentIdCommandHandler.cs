using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Orders.Commands.UpdateSessionIdAndPaymentIntentId
{
    internal class UpdateSessionIdAndPaymentIntentIdCommandHandler : IRequestHandler<UpdateSessionIdAndPaymentIntentIdCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSessionIdAndPaymentIntentIdCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSessionIdAndPaymentIntentIdCommand request, CancellationToken cancellationToken)
        {
            _orderRepository.UpdateStripePaymentId(request.orderId, request.sessionId, request.paymentIntentId);
            await _unitOfWork.SaveAsync();
        }
    }
}
