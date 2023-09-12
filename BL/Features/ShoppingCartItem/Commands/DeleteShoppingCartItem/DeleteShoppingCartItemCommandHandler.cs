using AutoMapper;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DeleteShoppingCartItem
{
    internal class DeleteShoppingCartItemCommandHandler : IRequestHandler<DeleteShoppingCartItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteShoppingCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            var shoppingCartItemForDelete = _mapper.Map<DAL.Models.ShoppingCartItem>(request.ShoppingCartItemForDelete);
            _unitOfWork.ShoppingCartItem.Delete(shoppingCartItemForDelete);
            await _unitOfWork.SaveAsync();
        }
    }
}
