using AutoMapper;
using BL.Features.GalleryImages.Commands.CreateGalleryImages;
using BL.Features.GalleryImages.Commands.DeleteGalleryImages;
using BL.Features.GalleryImages.Queries.GetGalleryImagesByProductId;
using BL.Features.ThumbnailImages.Commands.CreateThumbnailImage;
using BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;
using System.Transactions;

namespace BL.Features.Products.Commands.UpdateProduct
{
    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(
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

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productForUpdate = _mapper.Map<Product>(request.productForUpdate);
            int? oldThumbnailImageId = productForUpdate.ThumbnailImageId;

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (request.newThumbnailImage != null)
                {
                    var updatedThumbnailImageId = await _mediator.Send(new CreateThumbnailImageCommand(request.newThumbnailImage, productForUpdate.Name));
                    productForUpdate.ThumbnailImageId = updatedThumbnailImageId;
                }

                await _productRepository.UpdateAsync(productForUpdate);
                await _unitOfWork.SaveAsync();

                if (oldThumbnailImageId != null && request.newThumbnailImage != null)
                {
                    await _mediator.Send(new DeleteThumbnailImageCommand((int)oldThumbnailImageId));
                }

                if (request.newGalleryImages != null)
                {
                    var galleryImagesForDelete = await _mediator.Send(new GetGalleryImagesByProductIdQuery(productForUpdate.Id, false));
                    await _mediator.Send(new DeleteGalleryImagesCommand(galleryImagesForDelete));
                    await _mediator.Send(new CreateGalleryImagesCommand(request.newGalleryImages, productForUpdate.Id, productForUpdate.Name));
                }

                transaction.Complete();
            }

            return productForUpdate.Id;
        }
    }
}
