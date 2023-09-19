using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemById
{
    internal class GetShoppingCartItemByIdQueryHandler : IRequestHandler<GetShoppingCartItemByIdQuery, ShoppingCartItemDto?>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IMapper _mapper;

        public GetShoppingCartItemByIdQueryHandler(IShoppingCartItemRepository shoppingCartItemRepository, IMapper mapper)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _mapper = mapper;
        }

        public async Task<ShoppingCartItemDto?> Handle(GetShoppingCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedShoppingCartItem = await _shoppingCartItemRepository.GetFirstOrDefaultAsync(s => s.Id == request.shoppingCartItemId, isTracked:request.isTracked);
            return _mapper.Map<ShoppingCartItemDto?>(requestedShoppingCartItem);
        }
    }
}
