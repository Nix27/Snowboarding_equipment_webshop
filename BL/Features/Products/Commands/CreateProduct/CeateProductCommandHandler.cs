using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Commands.CreateProduct
{
    public class CeateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CeateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = _mapper.Map<Product>(request.newProduct);

            await _unitOfWork.Product.CreateAsync(newProduct);
            await _unitOfWork.SaveAsync();

            return newProduct.Id;
        }
    }
}
