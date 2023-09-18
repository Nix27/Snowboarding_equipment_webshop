using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.ThumbnailImages.Commands.CreateThumbnailImage
{
    internal class CreateThumbnailImageCommandHandler : IRequestHandler<CreateThumbnailImageCommand, int>
    {
        private readonly IThumbnailImageRepository _thumbnailImageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateThumbnailImageCommandHandler(IUnitOfWork unitOfWork,IThumbnailImageRepository thumbnailImageRepository)
        {
            _thumbnailImageRepository = thumbnailImageRepository;
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

            await _thumbnailImageRepository.CreateAsync(newThumbnailImage);
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
