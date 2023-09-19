using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Orders.Commands.UpdateOrder
{
    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderForUpdate = _mapper.Map<Order>(request.orderForUpdate);

            _orderRepository.Update(orderForUpdate);
            await _unitOfWork.SaveAsync();

            return orderForUpdate.Id;
        }
    }
}
