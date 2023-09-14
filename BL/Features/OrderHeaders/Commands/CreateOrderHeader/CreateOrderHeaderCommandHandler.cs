using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.OrderHeaders.Commands.CreateOrderHeader
{
    internal class CreateOrderHeaderCommandHandler : IRequestHandler<CreateOrderHeaderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderHeaderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOrderHeaderCommand request, CancellationToken cancellationToken)
        {
            var newOrderHeader = _mapper.Map<OrderHeader>(request.newOrderHeader);

            await _unitOfWork.OrderHeader.CreateAsync(newOrderHeader);
            await _unitOfWork.SaveAsync();

            return newOrderHeader.Id;
        }
    }
}
