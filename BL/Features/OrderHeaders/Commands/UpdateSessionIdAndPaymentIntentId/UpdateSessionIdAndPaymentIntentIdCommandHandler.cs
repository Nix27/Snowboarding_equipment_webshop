using AutoMapper;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderHeaders.Commands.UpdateSessionIdAndPaymentIntentId
{
    internal class UpdateSessionIdAndPaymentIntentIdCommandHandler : IRequestHandler<UpdateSessionIdAndPaymentIntentIdCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSessionIdAndPaymentIntentIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateSessionIdAndPaymentIntentIdCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.OrderHeader.UpdateStripePaymentId(request.orderHeaderId, request.sessionId, request.paymentIntentId);
            await _unitOfWork.SaveAsync();
        }
    }
}
