using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.GalleryImages.Queries.GetGalleryImageById
{
    internal class GetGalleryImageByIdQueryHandler : IRequestHandler<GetGalleryImageByIdQuery, GalleryImageDto>
    {
        private readonly IGalleryImageRepository _galleryImageRepository;
        private readonly IMapper _mapper;

        public GetGalleryImageByIdQueryHandler(IGalleryImageRepository galleryImageRepository, IMapper mapper)
        {
            _galleryImageRepository = galleryImageRepository;
            _mapper = mapper;
        }

        public async Task<GalleryImageDto> Handle(GetGalleryImageByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedGalleryImage = await _galleryImageRepository.GetFirstOrDefaultAsync(g => g.Id == request.id, isTracked:request.isTracked);

            if (requestedGalleryImage == null)
                throw new InvalidOperationException("Gallery image not found");
            
            return _mapper.Map<GalleryImageDto>(requestedGalleryImage);
        }
    }
}
