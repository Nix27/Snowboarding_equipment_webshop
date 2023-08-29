using AutoMapper;
using BL.Features.ThumbnailImages.Commands.CreateThumbnailImage;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage
{
    public class DeleteThumbnailImageCommandHandler : IRequestHandler<DeleteThumbnailImageCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteThumbnailImageCommand> _logger;

        public DeleteThumbnailImageCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DeleteThumbnailImageCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(DeleteThumbnailImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var thumbnailImageForDelete = _mapper.Map<ThumbnailImage>(request.thumbnailImageForDelete);

                _unitOfWork.ThumbnailImage.Delete(thumbnailImageForDelete);
                await _unitOfWork.SaveAsync();

                return thumbnailImageForDelete.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
