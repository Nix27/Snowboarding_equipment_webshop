using AutoMapper;
using BL.DTOs;
using BL.Features.GalleryImages.Commands;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.GalleryImages.Queries
{
    public class GetGalleryImagesByProductIdQueryHandler : IRequestHandler<GetGalleryImagesByProductIdQuery, IEnumerable<GalleryImageDto>?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetGalleryImagesByProductIdQuery> _logger;

        public GetGalleryImagesByProductIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetGalleryImagesByProductIdQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<GalleryImageDto>?> Handle(GetGalleryImagesByProductIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var galleryImages = await _unitOfWork.GalleryImage.GetAllAsync(g => g.Id == request.productId);
                return _mapper.Map<IEnumerable<GalleryImageDto>>(galleryImages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
