using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderDetails.Commands.CreateOrderDetail
{
    internal class CreateOrderDetailCommandHandler : IRequestHandler<CreateOrderDetailCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
        {
            var newOrderDetail = _mapper.Map<OrderDetail>(request.newOrderDetail);

            await _unitOfWork.OrderDetail.CreateAsync(newOrderDetail);
            await _unitOfWork.SaveAsync();

            return newOrderDetail.Id;
        }
    }
}
