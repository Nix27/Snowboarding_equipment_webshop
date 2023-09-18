using MediatR;

namespace BL.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(int productId) : IRequest<int>;
}
