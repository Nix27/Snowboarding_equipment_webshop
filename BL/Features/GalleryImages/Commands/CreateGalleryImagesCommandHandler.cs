using AutoMapper;
using BL.Features.Countries.Commands.CreateCountry;
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

namespace BL.Features.GalleryImages.Commands
{
    public class CreateGalleryImagesCommandHandler : IRequestHandler<CreateGalleryImagesCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateGalleryImagesCommand> _logger;

        public CreateGalleryImagesCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreateGalleryImagesCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateGalleryImagesCommand request, CancellationToken cancellationToken)
        {
            try
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

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return false;
            }
        }

        private byte[]? GetImageAsMemoryStream(IFormFile image)
        {
            if(image != null)
            {
                if(image.Length > 0)
                {
                    using(var memoryStream = new MemoryStream())
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
