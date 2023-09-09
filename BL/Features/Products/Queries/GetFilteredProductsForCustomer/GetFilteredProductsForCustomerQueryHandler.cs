using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Queries.GetFilteredProductsForCustomer
{
    public class GetFilteredProductsForCustomerQueryHandler : IRequestHandler<GetFilteredProductsForCustomerQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFilteredProductsForCustomerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetFilteredProductsForCustomerQuery request, CancellationToken cancellationToken)
        {
            var allProducts = await _unitOfWork.Product.GetAllAsync(includeProperties:"Category,ThumbnailImage");

            if(request.filter.MinPrice != null && request.filter.MaxPrice != null)
                allProducts = allProducts.Where(p => p.Price >= request.filter.MinPrice && p.Price <= request.filter.MaxPrice);

            if (request.filter.Categories != null)
                allProducts = allProducts.Where(p => request.filter.Categories.Contains(p.Category.Name));

            return _mapper.Map<IEnumerable<ProductDto>>(allProducts);
        }
    }
}
