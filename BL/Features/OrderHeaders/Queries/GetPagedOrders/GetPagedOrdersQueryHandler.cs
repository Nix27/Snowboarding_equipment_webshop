using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderHeaders.Queries.GetPagedOrders
{
    internal class GetPagedOrdersQueryHandler : IRequestHandler<GetPagedOrdersQuery, IEnumerable<OrderHeaderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderHeaderDto>> Handle(GetPagedOrdersQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await _unitOfWork.OrderHeader.GetAllAsync(includeProperties:"User");

            var pagedOrders = allOrders.Skip(request.page * request.size).Take(request.size);

            return _mapper.Map<IEnumerable<OrderHeaderDto>>(pagedOrders);
        }
    }
}
