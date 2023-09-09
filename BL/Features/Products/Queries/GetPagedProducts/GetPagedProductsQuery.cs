using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public record GetPagedProductsQuery(PageProductsRequestDto productsRequest, bool isTracked = true) : IRequest<IEnumerable<ProductDto>>;
}
