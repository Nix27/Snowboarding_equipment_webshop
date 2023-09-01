using AutoMapper;
using BL.DTOs;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.GalleryImages.Queries.GetGalleryImageById
{
    public class GetGalleryImageByIdQueryHandler : IRequestHandler<GetGalleryImageByIdQuery, GalleryImageDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetGalleryImageByIdQuery> _logger;

        public GetGalleryImageByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetGalleryImageByIdQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GalleryImageDto?> Handle(GetGalleryImageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedGalleryImage = await _unitOfWork.GalleryImage.GetFirstOrDefaultAsync(g => g.Id == request.id);
                return _mapper.Map<GalleryImageDto>(requestedGalleryImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
