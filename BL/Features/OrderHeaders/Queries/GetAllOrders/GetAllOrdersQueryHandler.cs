using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetAllOrders
{
    internal class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderHeaderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderHeaderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await _unitOfWork.OrderHeader.GetAllAsync(includeProperties:request.includeProperties);
            return _mapper.Map<IEnumerable<OrderHeaderDto>>(allOrders);
        }
    }
}
