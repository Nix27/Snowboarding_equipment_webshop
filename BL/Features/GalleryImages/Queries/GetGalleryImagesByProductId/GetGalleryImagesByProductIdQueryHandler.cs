using AutoMapper;
using BL.DTOs;
using DAL.Repositories.Interfaces;
using MediatR;

namespace BL.Features.GalleryImages.Queries.GetGalleryImagesByProductId
{
    internal class GetGalleryImagesByProductIdQueryHandler : IRequestHandler<GetGalleryImagesByProductIdQuery, IEnumerable<GalleryImageDto>>
    {
        private readonly IGalleryImageRepository _galleryImageRepository;
        private readonly IMapper _mapper;

        public GetGalleryImagesByProductIdQueryHandler(IGalleryImageRepository galleryImageRepository, IMapper mapper)
        {
            _galleryImageRepository = galleryImageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GalleryImageDto>> Handle(GetGalleryImagesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var requestedGalleryImages = await _galleryImageRepository.GetAllAsync(g => g.ProductId == request.productId, isTracked: request.isTracked);
            return _mapper.Map<IEnumerable<GalleryImageDto>>(requestedGalleryImages);
        }
    }
}
