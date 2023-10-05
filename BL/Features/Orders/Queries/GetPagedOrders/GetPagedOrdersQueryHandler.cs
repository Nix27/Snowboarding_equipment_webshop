using BL.DTOs;
using BL.Features.Orders.Queries.GetAllOrders;
using MediatR;
using Utilities.Constants.Status;

namespace BL.Features.Orders.Queries.GetPagedOrders
{
    internal class GetPagedOrdersQueryHandler : IRequestHandler<GetPagedOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IMediator _mediator;

        public GetPagedOrdersQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetPagedOrdersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<OrderDto> orders;

            switch (request.filterBy)
            {
                case OrderStatuses.StatusPending:
                    orders = await _mediator.Send(new GetAllOrdersQuery(o => o.OrderStatus == OrderStatuses.StatusPending)); break;
                case OrderStatuses.StatusApproved:
                    orders = await _mediator.Send(new GetAllOrdersQuery(o => o.OrderStatus == OrderStatuses.StatusApproved)); break;
                case OrderStatuses.StatusInProcess:
                    orders = await _mediator.Send(new GetAllOrdersQuery(o => o.OrderStatus == OrderStatuses.StatusInProcess)); break;
                case OrderStatuses.StatusShipped:
                    orders = await _mediator.Send(new GetAllOrdersQuery(o => o.OrderStatus == OrderStatuses.StatusShipped)); break;
                case OrderStatuses.StatusCancelled:
                    orders = await _mediator.Send(new GetAllOrdersQuery(o => o.OrderStatus == OrderStatuses.StatusCancelled)); break;
                default:
                    orders = await _mediator.Send(new GetAllOrdersQuery()); break;
            }

            var pagedOrders = orders.Skip((request.page - 1) * (int)request.size).Take((int)request.size);

            return pagedOrders;
        }
    }
}
