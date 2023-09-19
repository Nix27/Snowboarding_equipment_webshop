using AutoMapper;
using BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemById;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Commands.DeleteShoppingCartItem
{
    internal class DeleteShoppingCartItemCommandHandler : IRequestHandler<DeleteShoppingCartItemCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteShoppingCartItemCommandHandler(
            IMediator mediator,
            IShoppingCartItemRepository shoppingCartItemRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _mediator = mediator;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteShoppingCartItemCommand request, CancellationToken cancellationToken)
        {
            var shoppingCartItemForDeleteDto = await _mediator.Send(new GetShoppingCartItemByIdQuery(request.shoppingCartItemId, isTracked: false));
            
            if (shoppingCartItemForDeleteDto == null)
                throw new InvalidOperationException("Shopping cart item not found");

            var shoppingCartItemForDelete = _mapper.Map<DAL.Models.ShoppingCartItem>(shoppingCartItemForDeleteDto);

            _shoppingCartItemRepository.Delete(shoppingCartItemForDelete);
            await _unitOfWork.SaveAsync();

            return shoppingCartItemForDeleteDto.Id;
        }
    }
}
