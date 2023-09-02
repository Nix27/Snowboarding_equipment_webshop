using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.ThumbnailImages.Commands.CreateThumbnailImage
{
    public class CreateThumbnailImageCommandHandler : IRequestHandler<CreateThumbnailImageCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateThumbnailImageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateThumbnailImageCommand request, CancellationToken cancellationToken)
        {
            var imageAsStream = GetImageAsMemoryStream(request.newThumbnailImage) ??
                throw new InvalidOperationException("Image file can't be null");

            ThumbnailImage newThumbnailImage = new()
            {
                Content = Convert.ToBase64String(imageAsStream),
                Title = request.title
            };

            await _unitOfWork.ThumbnailImage.CreateAsync(newThumbnailImage);
            await _unitOfWork.SaveAsync();

            return newThumbnailImage.Id;
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
