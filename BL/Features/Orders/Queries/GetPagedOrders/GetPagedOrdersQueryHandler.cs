using AutoMapper;
using BL.DTOs;
using BL.Features.Orders.Queries.GetAllOrders;
using MediatR;

namespace BL.Features.Orders.Queries.GetPagedOrders
{
    internal class GetPagedOrdersQueryHandler : IRequestHandler<GetPagedOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetPagedOrdersQueryHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetPagedOrdersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OrderDto>? orders = request.orders;

            if(orders == null)
            {
                var ordersFromDb = await _mediator.Send(new GetAllOrdersQuery(includeProperties:"User"));
                orders = _mapper.Map<IEnumerable<OrderDto>>(ordersFromDb);
            }

            var pagedOrders = orders.Skip(request.page * request.size).Take(request.size);

            return pagedOrders;
        }
    }
}
