using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemById
{
    internal class GetShoppingCartItemByIdQueryHandler : IRequestHandler<GetShoppingCartItemByIdQuery, ShoppingCartItemDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetShoppingCartItemByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShoppingCartItemDto?> Handle(GetShoppingCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedShoppingCartItem = await _unitOfWork.ShoppingCartItem.GetFirstOrDefaultAsync(s => s.Id == request.shoppingCartItemId, isTracked:request.isTracked);
            return _mapper.Map<ShoppingCartItemDto?>(requestedShoppingCartItem);
        }
    }
}
