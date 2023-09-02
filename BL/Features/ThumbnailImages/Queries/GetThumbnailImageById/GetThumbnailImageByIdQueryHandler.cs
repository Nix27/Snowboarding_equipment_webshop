using AutoMapper;
using BL.DTOs;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ThumbnailImages.Queries.GetThumbnailImageById
{
    public class GetThumbnailImageByIdQueryHandler : IRequestHandler<GetThumbnailImageByIdQuery, ThumbnailImageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetThumbnailImageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ThumbnailImageDto> Handle(GetThumbnailImageByIdQuery request, CancellationToken cancellationToken)
        {
            var requestedThumbnailImage = await _unitOfWork.ThumbnailImage.GetFirstOrDefaultAsync(t => t.Id == request.id, isTracked: request.isTracked);
            return _mapper.Map<ThumbnailImageDto>(requestedThumbnailImage);
        }
    }
}
