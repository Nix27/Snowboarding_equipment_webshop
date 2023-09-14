using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetAllShoppingCartItemsForUser
{
    internal class GetAllShoppingCartItemsForUserQueryHandler : IRequestHandler<GetAllShoppingCartItemsForUserQuery, IEnumerable<ShoppingCartItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllShoppingCartItemsForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShoppingCartItemDto>> Handle(GetAllShoppingCartItemsForUserQuery request, CancellationToken cancellationToken)
        {
            var shoppingCartItemsForUser = await _unitOfWork.ShoppingCartItem.GetAllAsync(s => s.UserId == request.userId, includeProperties:"Product", isTracked:request.isTracked);
            return _mapper.Map<IEnumerable<ShoppingCartItemDto>>(shoppingCartItemsForUser);
        }
    }
}
