using BL.Features.Countries.Queries.GetAllCountries;
using BL.Features.Orders.Queries.GetAllOrders;
using MediatR;

namespace BL.Features.Orders.Queries.GetNumberOfOrders
{
    internal class GetNumberOfOrdersQueryHandler : IRequestHandler<GetNumberOfOrdersQuery, int>
    {
        private readonly IMediator _mediator;

        public GetNumberOfOrdersQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> Handle(GetNumberOfOrdersQuery request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetAllOrdersQuery(request.filter, isTracked: false))
                            .GetAwaiter()
                            .GetResult()
                            .Count();
        }
    }
}
