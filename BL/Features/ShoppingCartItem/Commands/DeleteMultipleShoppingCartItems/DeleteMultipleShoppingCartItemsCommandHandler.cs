using AutoMapper;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DeleteMultipleShoppingCartItems
{
    internal class DeleteMultipleShoppingCartItemsCommandHandler : IRequestHandler<DeleteMultipleShoppingCartItemsCommand>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteMultipleShoppingCartItemsCommandHandler(IShoppingCartItemRepository shoppingCartItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteMultipleShoppingCartItemsCommand request, CancellationToken cancellationToken)
        {
            var shoppingCartItemsForDelete = _mapper.Map<IEnumerable<DAL.Models.ShoppingCartItem>>(request.shoppingCartItemsForDelete);
            _shoppingCartItemRepository.DeleteMultiple(shoppingCartItemsForDelete);
            await _unitOfWork.SaveAsync();
        }
    }
}
