using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetAllShoppingCartItemsForUser
{
    internal class GetAllShoppingCartItemsForUserQueryHandler : IRequestHandler<GetAllShoppingCartItemsForUserQuery, IEnumerable<ShoppingCartItemDto>>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IMapper _mapper;

        public GetAllShoppingCartItemsForUserQueryHandler(IShoppingCartItemRepository shoppingCartItemRepository, IMapper mapper)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShoppingCartItemDto>> Handle(GetAllShoppingCartItemsForUserQuery request, CancellationToken cancellationToken)
        {
            var shoppingCartItemsForUser = await _shoppingCartItemRepository.GetAllAsync(s => s.UserId == request.userId, includeProperties:request.includeProperties, isTracked:request.isTracked);
            return _mapper.Map<IEnumerable<ShoppingCartItemDto>>(shoppingCartItemsForUser);
        }
    }
}
