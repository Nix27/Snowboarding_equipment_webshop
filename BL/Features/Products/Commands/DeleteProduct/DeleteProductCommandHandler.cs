using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Commands.DeleteProduct
{
    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productForDelete = _mapper.Map<Product>(request.productForDelete);

            _unitOfWork.Product.Delete(productForDelete);
            await _unitOfWork.SaveAsync();

            return productForDelete.Id;
        }
    }
}
