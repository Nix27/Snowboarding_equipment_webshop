using AutoMapper;
using BL.DTOs;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.ThumbnailImages.Queries.GetThumbnailImageById
{
    internal class GetThumbnailImageByIdQueryHandler : IRequestHandler<GetThumbnailImageByIdQuery, ThumbnailImageDto>
    {
        private readonly IThumbnailImageRepository _thumbnailImageRepository;
        private readonly IMapper _mapper;

        public GetThumbnailImageByIdQueryHandler(IThumbnailImageRepository thumbnailImageRepository, IMapper mapper)
        {
            _thumbnailImageRepository = thumbnailImageRepository;
            _mapper = mapper;
        }

        public async Task<ThumbnailImageDto> Handle(GetThumbnailImageByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedThumbnailImage = await _thumbnailImageRepository.GetFirstOrDefaultAsync(t => t.Id == request.id, isTracked: request.isTracked);

            if (requestedThumbnailImage == null)
                throw new InvalidOperationException("Thumbnail image not found");
            
            return _mapper.Map<ThumbnailImageDto>(requestedThumbnailImage);
        }
    }
}
