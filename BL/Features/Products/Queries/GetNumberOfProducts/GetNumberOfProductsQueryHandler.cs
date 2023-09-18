using BL.Features.Products.Queries.GetAllProducts;
using MediatR;

namespace BL.Features.Products.Queries.GetNumberOfProducts
{
    internal class GetNumberOfProductsQueryHandler : IRequestHandler<GetNumberOfProductsQuery, int>
    {
        private readonly IMediator _mediator;

        public GetNumberOfProductsQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> Handle(GetNumberOfProductsQuery request, CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetAllProductsQuery(request.filter, isTracked: false))
                            .GetAwaiter()
                            .GetResult()
                            .Count();
        }
    }
}
