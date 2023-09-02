using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.GalleryImages.Queries.GetGalleryImagesByProductId
{
    public class GetGalleryImagesByProductIdQueryHandler : IRequestHandler<GetGalleryImagesByProductIdQuery, IEnumerable<GalleryImageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGalleryImagesByProductIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GalleryImageDto>> Handle(GetGalleryImagesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var requestedGalleryImages = await _unitOfWork.GalleryImage.GetAllAsync(g => g.ProductId == request.productId, isTracked: request.isTracked);
            return _mapper.Map<IEnumerable<GalleryImageDto>>(requestedGalleryImages);
        }
    }
}
