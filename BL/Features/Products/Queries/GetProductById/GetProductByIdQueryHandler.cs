using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.Products.Queries.GetProductById
{
    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedProduct = await _productRepository.GetFirstOrDefaultAsync(p => p.Id == request.id, includeProperties:request.includeProperties, isTracked:request.isTracked);

            if (requestedProduct == null) 
                throw new InvalidOperationException("Product not found");

            return _mapper.Map<ProductDto>(requestedProduct);
        }
    }
}
