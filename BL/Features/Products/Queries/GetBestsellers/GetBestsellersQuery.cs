using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetBestsellers
{
    public record GetBestsellersQuery() : IRequest<IEnumerable<ProductDto>>;
}
