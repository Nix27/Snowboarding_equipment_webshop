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

            if (request.productsRequest.SearchTerm != null)
            {
                request.productsRequest.SearchBy = request.productsRequest.SearchBy ?? "name";

                if (String.Compare(request.productsRequest.SearchBy, "name", true) == 0)
                {
                    allProducts = allProducts.Where(p => p.Name.ToLower().Contains(request.productsRequest.SearchTerm.ToLower()));
                }
                else if (String.Compare(request.productsRequest.SearchBy, "category", true) == 0)
                {
                    allProducts = allProducts.Where(p => p.Category.Name.ToLower().Contains(request.productsRequest.SearchTerm.ToLower()));
                }
            }

            var pagedProducts = allProducts.Skip(request.productsRequest.Page * request.productsRequest.Size).Take(request.productsRequest.Size);

            return _mapper.Map<IEnumerable<ProductDto>>(pagedProducts);
        }
    }
}
