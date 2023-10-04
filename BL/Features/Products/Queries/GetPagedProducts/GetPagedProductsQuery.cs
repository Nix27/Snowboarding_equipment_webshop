using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public record GetPagedProductsQuery(
        IEnumerable<ProductDto> products, 
        int page, 
        float size, 
        string? includeProperties = null) : IRequest<IEnumerable<ProductDto>>;
}
