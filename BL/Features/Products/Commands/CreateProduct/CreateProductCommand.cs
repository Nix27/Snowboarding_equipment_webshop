using BL.DTOs;
using MediatR;

namespace BL.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(ProductDto newProduct) : IRequest<int>;
}
