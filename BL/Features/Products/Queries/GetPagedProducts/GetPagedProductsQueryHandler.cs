using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
        {
            var allProducts = await _unitOfWork.Product.GetAllAsync(includeProperties: "Category,ThumbnailImage", isTracked: request.isTracked);

            if (request.searchTerm != null)
            {
                if (String.Compare(request.filterBy, "name", true) == 0)
                {
                    allProducts = allProducts.Where(p => p.Name.ToLower().Contains(request.searchTerm.ToLower()));
                }
                else if (String.Compare(request.filterBy, "category", true) == 0)
                {
                    allProducts = allProducts.Where(p => p.Category.Name.ToLower().Contains(request.searchTerm.ToLower()));
                }
            }

            var pagedProducts = allProducts.Skip(request.page * request.size).Take(request.size);

            return _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);
        }
    }
}
