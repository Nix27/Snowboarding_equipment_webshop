using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetNumberOfShoppingCartItemsForUser
{
    internal class GetNumberOfShoppingCartItemsForUserQueryHandler : IRequestHandler<GetNumberOfShoppingCartItemsForUserQuery, int>
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        public GetNumberOfShoppingCartItemsForUserQueryHandler(IShoppingCartItemRepository shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }

        public async Task<int> Handle(GetNumberOfShoppingCartItemsForUserQuery request, CancellationToken cancellationToken)
        {
            return _shoppingCartItemRepository.GetAllAsync(s => s.UserId == request.userId)
                                              .GetAwaiter()
                                              .GetResult()
                                              .Count();
        }
    }
}
