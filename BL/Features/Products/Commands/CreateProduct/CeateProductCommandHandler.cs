using AutoMapper;
using BL.Features.GalleryImages.Commands.CreateGalleryImages;
using BL.Features.ThumbnailImages.Commands.CreateThumbnailImage;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;
using System.Transactions;

namespace BL.Features.Products.Commands.CreateProduct
{
    internal class CeateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CeateProductCommandHandler(
            IMediator mediator, 
            IProductRepository productRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _mediator = mediator;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = _mapper.Map<Product>(request.newProduct);

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if(request.newThumbnailImage != null)
                {
                    var createdThumbnailImageId = await _mediator.Send(new CreateThumbnailImageCommand(request.newThumbnailImage, newProduct.Name));
                    newProduct.ThumbnailImageId = createdThumbnailImageId;
                }

                await _productRepository.CreateAsync(newProduct);
                await _unitOfWork.SaveAsync();

                if (request.newGalleryImages != null)
                {
                    await _mediator.Send(new CreateGalleryImagesCommand(request.newGalleryImages, newProduct.Id, newProduct.Name));
                }

                transaction.Complete();
            }

            return newProduct.Id;
        }
    }
}
