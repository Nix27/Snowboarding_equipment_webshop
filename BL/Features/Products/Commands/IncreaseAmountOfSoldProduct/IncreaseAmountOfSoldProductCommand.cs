using MediatR;

namespace BL.Features.Products.Commands.IncreaseAmountOfSoldProduct
{
    public record IncreaseAmountOfSoldProductCommand(IDictionary<int, int> productsAndQuantities) : IRequest;
}
