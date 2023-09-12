using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage
{
    internal class DeleteThumbnailImageCommandHandler : IRequestHandler<DeleteThumbnailImageCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteThumbnailImageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteThumbnailImageCommand request, CancellationToken cancellationToken)
        {
            var thumbnailImageForDelete = _mapper.Map<ThumbnailImage>(request.thumbnailImageForDelete);

            _unitOfWork.ThumbnailImage.Delete(thumbnailImageForDelete);
            await _unitOfWork.SaveAsync();

            return thumbnailImageForDelete.Id;
        }
    }
}
