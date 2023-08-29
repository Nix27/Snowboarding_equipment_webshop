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

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, IEnumerable<ProductDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPagedProductsQuery> _logger;

        public GetPagedProductsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetPagedProductsQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>?> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allProducts = await _unitOfWork.Product.GetAllAsync(includeProperties:"Category");

                if(request.searchTerm != null)
                {
                    if(String.Compare(request.filterBy, "name", true) == 0)
                    {
                        allProducts = allProducts.Where(p => p.Name.ToLower().Contains(request.searchTerm.ToLower()));
                    }
                    else if(String.Compare(request.filterBy, "category", true) == 0)
                    {
                        allProducts = allProducts.Where(p => p.Category.Name.ToLower().Contains(request.searchTerm.ToLower()));
                    }
                }

                var pagedProducts = allProducts.Skip(request.page * request.size).Take(request.size);

                return _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
