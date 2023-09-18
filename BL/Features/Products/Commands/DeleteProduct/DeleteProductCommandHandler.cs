using AutoMapper;
using BL.Features.Products.Queries.GetProductById;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.Products.Commands.DeleteProduct
{
    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IMediator mediator, IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mediator = mediator;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productForDeleteDto = await _mediator.Send(new GetProductByIdQuery(request.productId, isTracked:false));

            var productForDelete = _mapper.Map<Product>(productForDeleteDto);

            _productRepository.Delete(productForDelete);
            await _unitOfWork.SaveAsync();

            return productForDeleteDto.Id;
        }
    }
}
