using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using MediatR;

namespace BL.Features.Products.Queries.GetFilteredProductsForCustomer
{
    internal class GetFilteredProductsForCustomerQueryHandler : IRequestHandler<GetFilteredProductsForCustomerQuery, IEnumerable<ProductDto>>
    {
        private readonly IMediator _mediator;

        public GetFilteredProductsForCustomerQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetFilteredProductsForCustomerQuery request, CancellationToken cancellationToken)
        {
            var allProducts = await _mediator.Send(new GetAllProductsQuery(includeProperties: "Category,ThumbnailImage"));

            if(request.filter.MinPrice != null && request.filter.MaxPrice != null)
                allProducts = allProducts.Where(p => p.Price >= request.filter.MinPrice && p.Price <= request.filter.MaxPrice);

            if (request.filter.Categories != null)
                allProducts = allProducts.Where(p => request.filter.Categories.Contains(p.Category.Name));

            return allProducts;
        }
    }
}
