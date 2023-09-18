using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.GalleryImages.Commands.CreateGalleryImages
{
    internal class CreateGalleryImagesCommandHandler : IRequestHandler<CreateGalleryImagesCommand>
    {
        private readonly IGalleryImageRepository _galleryImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGalleryImagesCommandHandler(IGalleryImageRepository galleryImageRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _galleryImageRepository = galleryImageRepository;
        }

        public async Task Handle(CreateGalleryImagesCommand request, CancellationToken cancellationToken)
        {
            foreach (var image in request.newGalleryImages)
            {
                var imageAsStream = GetImageAsMemoryStream(image) ??
                    throw new InvalidOperationException("Image can't be null");

                GalleryImage newGalleryImage = new()
                {
                    Content = Convert.ToBase64String(imageAsStream),
                    Title = request.title,
                    ProductId = request.productId
                };

                await _galleryImageRepository.CreateAsync(newGalleryImage);
            }

            await _unitOfWork.SaveAsync();
        }

        private byte[]? GetImageAsMemoryStream(IFormFile image)
        {
            if (image != null)
            {
                if (image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }

            return null;
        }
    }
}
