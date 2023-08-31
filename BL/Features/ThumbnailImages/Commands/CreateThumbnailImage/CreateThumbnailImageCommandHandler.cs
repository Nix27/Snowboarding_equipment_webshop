using AutoMapper;
using BL.Features.Products.Commands.CreateProduct;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Commands.CreateThumbnailImage
{
    public class CreateThumbnailImageCommandHandler : IRequestHandler<CreateThumbnailImageCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateThumbnailImageCommand> _logger;

        public CreateThumbnailImageCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreateThumbnailImageCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(CreateThumbnailImageCommand request, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
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
