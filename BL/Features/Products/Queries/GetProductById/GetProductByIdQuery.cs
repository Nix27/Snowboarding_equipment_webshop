using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int id, string? includeProperties = null, bool isTracked = true) : IRequest<ProductDto>;
}
