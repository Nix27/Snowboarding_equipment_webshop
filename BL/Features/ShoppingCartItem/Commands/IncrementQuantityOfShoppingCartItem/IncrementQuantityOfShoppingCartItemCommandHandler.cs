using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem
{
    internal class IncrementQuantityOfShoppingCartItemCommandHandler : IRequestHandler<IncrementQuantityOfShoppingCartItemCommand>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IncrementQuantityOfShoppingCartItemCommandHandler(IShoppingCartItemRepository shoppingCartItemRepository, IUnitOfWork unitOfWork)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(IncrementQuantityOfShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            await _shoppingCartItemRepository.IncrementQuantity(request.shoppingCartItemId, request.quantity);
            await _unitOfWork.SaveAsync();
        }
    }
}
