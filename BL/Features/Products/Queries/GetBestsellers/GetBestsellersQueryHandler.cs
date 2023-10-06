using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using MediatR;

namespace BL.Features.Products.Queries.GetBestsellers
{
    internal class GetBestsellersQueryHandler : IRequestHandler<GetBestsellersQuery, IEnumerable<ProductDto>>
    {
        private readonly IMediator _mediator;

        public GetBestsellersQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetBestsellersQuery request, CancellationToken cancellationToken)
        {
            var products = await _mediator.Send(new GetAllProductsQuery());

            products = products.OrderBy(p => p.AmountOfSold).Reverse().Take(4);

            return products;
        }
    }
}
