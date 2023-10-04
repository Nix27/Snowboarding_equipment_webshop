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

        public Task<IEnumerable<ProductDto>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
        {
            var pagedProducts = request.products.Skip((request.page - 1) * (int)request.size).Take((int)request.size);

            return Task.FromResult(pagedProducts);
        }
    }
}
