using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productForUpdate = _mapper.Map<Product>(request.productForUpdate);

            await _unitOfWork.Product.UpdateAsync(productForUpdate);
            await _unitOfWork.SaveAsync();

            return productForUpdate.Id;
        }
    }
}
