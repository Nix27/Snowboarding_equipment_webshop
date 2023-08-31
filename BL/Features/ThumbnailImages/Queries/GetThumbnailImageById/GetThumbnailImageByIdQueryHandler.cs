using AutoMapper;
using BL.DTOs;
using BL.Features.ThumbnailImages.Commands.CreateThumbnailImage;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Queries.GetThumbnailImageById
{
    public class GetThumbnailImageByIdQueryHandler : IRequestHandler<GetThumbnailImageByIdQuery, ThumbnailImageDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetThumbnailImageByIdQuery> _logger;

        public GetThumbnailImageByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GetThumbnailImageByIdQuery> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ThumbnailImageDto?> Handle(GetThumbnailImageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedThumbnailImage = await _unitOfWork.ThumbnailImage.GetFirstOrDefaultAsync(t => t.Id == request.id);
                return _mapper.Map<ThumbnailImageDto>(requestedThumbnailImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
