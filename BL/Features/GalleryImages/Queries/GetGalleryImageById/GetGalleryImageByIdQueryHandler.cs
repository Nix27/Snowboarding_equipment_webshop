using AutoMapper;
using BL.DTOs;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.GalleryImages.Queries.GetGalleryImageById
{
    public class GetGalleryImageByIdQueryHandler : IRequestHandler<GetGalleryImageByIdQuery, GalleryImageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGalleryImageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GalleryImageDto> Handle(GetGalleryImageByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedGalleryImage = await _unitOfWork.GalleryImage.GetFirstOrDefaultAsync(g => g.Id == request.id);
            return _mapper.Map<GalleryImageDto>(requestedGalleryImage);
        }
    }
}
