using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    internal class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IMediator _mediator;

        public GetPagedProductsQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _mediator.Send(new GetAllProductsQuery(includeProperties:request.includeProperties));

            if (!String.IsNullOrEmpty(request.pageRequest.SearchTerm))
            {
                if(request.pageRequest.SearchBy == "category")
                {
                    products = products.Where(p => p.Category.Name.ToLower().Contains(request.pageRequest.SearchTerm.ToLower()));
                }
                else
                {
                    products = products.Where(p => p.Name.ToLower().Contains(request.pageRequest.SearchTerm.ToLower()));
                }
            }

            var pagedProducts = products.Skip((request.pageRequest.Page - 1) * (int)request.pageRequest.Size).Take((int)request.pageRequest.Size);

            return pagedProducts;
        }
    }
}
