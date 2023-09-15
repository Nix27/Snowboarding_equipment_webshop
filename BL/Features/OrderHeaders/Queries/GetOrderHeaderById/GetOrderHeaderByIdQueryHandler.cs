using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetOrderHeaderById
{
    internal class GetOrderHeaderByIdQueryHandler : IRequestHandler<GetOrderHeaderByIdQuery, OrderHeaderDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderHeaderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderHeaderDto?> Handle(GetOrderHeaderByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedOrderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefaultAsync(o => o.Id == request.orderHeaderId, includeProperties:request.includeProperties);
            return _mapper.Map<OrderHeaderDto?>(requestedOrderHeader);
        }
    }
}
