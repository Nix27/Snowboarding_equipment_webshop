using AutoMapper;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage
{
    internal class DeleteThumbnailImageCommandHandler : IRequestHandler<DeleteThumbnailImageCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly IThumbnailImageRepository _thumbnailImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteThumbnailImageCommandHandler(IMediator mediator, IThumbnailImageRepository thumbnailImageRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mediator = mediator;
            _thumbnailImageRepository = thumbnailImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteThumbnailImageCommand request, CancellationToken cancellationToken)
        {
            var thumbnailImageForDeleteDto = await _mediator.Send(new GetThumbnailImageByIdQuery(request.thumbnailImageId, isTracked:false));

            var thumbnailImageForDelete = _mapper.Map<ThumbnailImage>(thumbnailImageForDeleteDto);

            _thumbnailImageRepository.Delete(thumbnailImageForDelete);
            await _unitOfWork.SaveAsync();

            return thumbnailImageForDeleteDto.Id;
        }
    }
}
