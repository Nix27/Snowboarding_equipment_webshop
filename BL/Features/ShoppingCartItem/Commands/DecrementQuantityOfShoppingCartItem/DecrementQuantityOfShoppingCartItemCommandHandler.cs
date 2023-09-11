using AutoMapper;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DecrementQuantityOfShoppingCartItem
{
    public class DecrementQuantityOfShoppingCartItemCommandHandler : IRequestHandler<DecrementQuantityOfShoppingCartItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DecrementQuantityOfShoppingCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DecrementQuantityOfShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ShoppingCartItem.DecrementQuantity(request.shoppingCartItemId, request.quantity);
            await _unitOfWork.SaveAsync();
        }
    }
}
