using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    internal class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetPagedProductsQueryHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ProductDto>? products = request.products;

            if (products == null)
            {
                var productsFromDb = await _mediator.Send(new GetAllProductsQuery(includeProperties:request.includeProperties));
                products = _mapper.Map<IEnumerable<ProductDto>>(productsFromDb);
            }

            var pagedProducts = products.Skip(request.page * request.size).Take(request.size);

            return pagedProducts;
        }
    }
}
