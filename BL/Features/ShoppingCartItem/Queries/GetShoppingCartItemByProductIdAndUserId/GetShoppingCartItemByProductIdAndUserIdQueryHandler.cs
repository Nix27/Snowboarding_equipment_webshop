using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemByProductIdAndUserId
{
    internal class GetShoppingCartItemByProductIdAndUserIdQueryHandler : IRequestHandler<GetShoppingCartItemByProductIdAndUserIdQuery, ShoppingCartItemDto?>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IMapper _mapper;

        public GetShoppingCartItemByProductIdAndUserIdQueryHandler(IShoppingCartItemRepository shoppingCartItemRepository, IMapper mapper)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _mapper = mapper;
        }

        public async Task<ShoppingCartItemDto?> Handle(GetShoppingCartItemByProductIdAndUserIdQuery request, CancellationToken cancellationToken)
        {
            var requestedShoppingcartItem = await _shoppingCartItemRepository.GetFirstOrDefaultAsync(s => s.ProductId == request.productId && s.UserId == request.userId);
            return _mapper.Map<ShoppingCartItemDto?>(requestedShoppingcartItem);
        }
    }
}
