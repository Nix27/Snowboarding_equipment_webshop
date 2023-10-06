using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Commands.IncreaseAmountOfSoldProduct
{
    internal class IncreaseAmountOfSoldProductCommandHandler : IRequestHandler<IncreaseAmountOfSoldProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IncreaseAmountOfSoldProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(IncreaseAmountOfSoldProductCommand request, CancellationToken cancellationToken)
        {
            foreach(var productAndQuantity in request.productsAndQuantities)
            {
                await _productRepository.IncreaseAmountOfSoldAsync(productAndQuantity.Key, productAndQuantity.Value);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
