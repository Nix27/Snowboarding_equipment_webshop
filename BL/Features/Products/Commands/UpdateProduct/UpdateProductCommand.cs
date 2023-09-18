using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(ProductDto productForUpdate) : IRequest<int>;
}
