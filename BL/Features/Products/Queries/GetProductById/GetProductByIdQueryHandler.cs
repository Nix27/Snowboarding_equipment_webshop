using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Queries.GetProductById
{
    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedProduct = await _unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == request.id, includeProperties: "Category,ThumbnailImage,GalleryImages", isTracked: request.isTracked);

            if (requestedProduct == null) return null;

            return _mapper.Map<ProductDto?>(requestedProduct);
        }
    }
}
