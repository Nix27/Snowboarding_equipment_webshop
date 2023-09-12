using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.GalleryImages.Commands.DeleteGalleryImages
{
    internal class DeleteGalleryImagesCommandHandler : IRequestHandler<DeleteGalleryImagesCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteGalleryImagesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteGalleryImagesCommand request, CancellationToken cancellationToken)
        {
            var galleryImagesForDelete = _mapper.Map<IEnumerable<GalleryImage>>(request.galleryImagesForDelete);
            _unitOfWork.GalleryImage.DeleteMultiple(galleryImagesForDelete);
            await _unitOfWork.SaveAsync();
        }
    }
}
