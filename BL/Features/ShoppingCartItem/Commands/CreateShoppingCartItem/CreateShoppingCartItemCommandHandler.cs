using AutoMapper;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.CreateShoppingCartItem
{
    internal class CreateShoppingCartItemCommandHandler : IRequestHandler<CreateShoppingCartItemCommand, int>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateShoppingCartItemCommandHandler(IShoppingCartItemRepository shoppingCartItemRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            var newShoppingCartItem = _mapper.Map<DAL.Models.ShoppingCartItem>(request.newShoppingCartItem);

            await _shoppingCartItemRepository.CreateAsync(newShoppingCartItem);
            await _unitOfWork.SaveAsync();

            return newShoppingCartItem.Id;
        }
    }
}
