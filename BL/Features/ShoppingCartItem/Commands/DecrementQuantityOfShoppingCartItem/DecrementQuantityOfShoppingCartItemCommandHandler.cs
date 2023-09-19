using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DecrementQuantityOfShoppingCartItem
{
    internal class DecrementQuantityOfShoppingCartItemCommandHandler : IRequestHandler<DecrementQuantityOfShoppingCartItemCommand>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DecrementQuantityOfShoppingCartItemCommandHandler(IShoppingCartItemRepository shoppingCartItemRepository, IUnitOfWork unitOfWork)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DecrementQuantityOfShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            await _shoppingCartItemRepository.DecrementQuantity(request.shoppingCartItemId, request.quantity);
            await _unitOfWork.SaveAsync();
        }
    }
}
