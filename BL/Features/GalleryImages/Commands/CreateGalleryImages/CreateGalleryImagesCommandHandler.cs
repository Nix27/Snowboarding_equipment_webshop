using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.GalleryImages.Commands.CreateGalleryImages
{
    public class CreateGalleryImagesCommandHandler : IRequestHandler<CreateGalleryImagesCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateGalleryImagesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CreateGalleryImagesCommand request, CancellationToken cancellationToken)
        {
            foreach (var image in request.newGalleryImages)
            {
                var imageAsStream = GetImageAsMemoryStream(image);

                if (imageAsStream == null)
                    throw new InvalidOperationException("Image can't be null");

                GalleryImage newGalleryImage = new()
                {
                    Content = Convert.ToBase64String(imageAsStream),
                    Title = request.title,
                    ProductId = request.productId
                };

                await _unitOfWork.GalleryImage.CreateAsync(newGalleryImage);
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
