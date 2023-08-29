using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Commands.CreateProduct;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductsQuery> _logger;

        public GetAllProductsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetAllProductsQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>?> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allProducts = await _unitOfWork.Product.GetAllAsync(request.filter);
                return _mapper.Map<IEnumerable<ProductDto>>(allProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
