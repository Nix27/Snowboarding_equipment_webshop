using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.Orders.Queries.GetOrderById
{
    internal class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedOrder = await _orderRepository.GetFirstOrDefaultAsync(o => o.Id == request.orderId, isTracked:request.isTracked, includeProperties:request.includeProperties);

            if (requestedOrder == null)
                throw new InvalidOperationException("Order not found");
            
            return _mapper.Map<OrderDto>(requestedOrder);
        }
    }
}
