using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Orders.Commands.CreateOrder
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = _mapper.Map<Order>(request.newOrder);

            await _orderRepository.CreateAsync(newOrder);
            await _unitOfWork.SaveAsync();

            return newOrder.Id;
        }
    }
}
