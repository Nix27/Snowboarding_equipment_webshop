using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderItems.Commands.CreateOrderItem
{
    internal class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, int>
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderItemCommandHandler(IOrderItemRepository orderItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var newOrderItem = _mapper.Map<OrderItem>(request.newOrderItem);

            await _orderItemRepository.CreateAsync(newOrderItem);
            await _unitOfWork.SaveAsync();

            return newOrderItem.Id;
        }
    }
}
