using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetPagedProducts
{
    public record GetPagedProductsQuery(int page, int size, string filterBy, string? searchTerm, bool isTracked = true) : IRequest<IEnumerable<ProductDto>>;
}
