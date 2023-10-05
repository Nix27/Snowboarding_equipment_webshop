using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public record GetPagedProductsQuery(PageProductsRequest pageRequest, string? includeProperties = null) : IRequest<IEnumerable<ProductDto>>;
}
