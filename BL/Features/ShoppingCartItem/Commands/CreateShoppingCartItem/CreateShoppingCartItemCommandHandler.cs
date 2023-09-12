using AutoMapper;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.CreateShoppingCartItem
{
    internal class CreateShoppingCartItemCommandHandler : IRequestHandler<CreateShoppingCartItemCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateShoppingCartItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            var newShoppingCartItem = _mapper.Map<DAL.Models.ShoppingCartItem>(request.newShoppingCartItem);

            await _unitOfWork.ShoppingCartItem.CreateAsync(newShoppingCartItem);
            await _unitOfWork.SaveAsync();

            return newShoppingCartItem.Id;
        }
    }
}
