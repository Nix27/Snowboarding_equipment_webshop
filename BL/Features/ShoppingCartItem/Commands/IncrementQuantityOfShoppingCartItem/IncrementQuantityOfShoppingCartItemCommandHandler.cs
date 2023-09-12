using AutoMapper;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem
{
    internal class IncrementQuantityOfShoppingCartItemCommandHandler : IRequestHandler<IncrementQuantityOfShoppingCartItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IncrementQuantityOfShoppingCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(IncrementQuantityOfShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ShoppingCartItem.IncrementQuantity(request.shoppingCartItemId, request.quantity);
            await _unitOfWork.SaveAsync();
        }
    }
}
