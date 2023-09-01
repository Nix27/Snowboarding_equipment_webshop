using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductByIdQuery> _logger;

        public GetProductByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetProductByIdQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedProduct = await _unitOfWork.Product.GetFirstOrDefaultAsync(p => p.Id == request.id, isTracked: request.isTracked, includeProperties: "Category,ThumbnailImage,GalleryImages");
                return _mapper.Map<ProductDto>(requestedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
