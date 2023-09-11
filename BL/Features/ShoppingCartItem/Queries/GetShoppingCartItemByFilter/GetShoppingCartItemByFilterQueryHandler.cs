using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemByFilter
{
    public class GetShoppingCartItemByFilterQueryHandler : IRequestHandler<GetShoppingCartItemByFilterQuery, ShoppingCartItemDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetShoppingCartItemByFilterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShoppingCartItemDto?> Handle(GetShoppingCartItemByFilterQuery request, CancellationToken cancellationToken)
        {
            var requestedShoppingcartItem = await _unitOfWork.ShoppingCartItem.GetFirstOrDefaultAsync(request.filter);
            return _mapper.Map<ShoppingCartItemDto?>(requestedShoppingcartItem);
        }
    }
}
