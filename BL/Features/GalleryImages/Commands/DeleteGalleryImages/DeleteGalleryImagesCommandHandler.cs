using AutoMapper;
using DAL.Models;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using MediatR;

namespace BL.Features.GalleryImages.Commands.DeleteGalleryImages
{
    internal class DeleteGalleryImagesCommandHandler : IRequestHandler<DeleteGalleryImagesCommand>
    {
        private readonly IGalleryImageRepository _galleryImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteGalleryImagesCommandHandler(IGalleryImageRepository galleryImageRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _galleryImageRepository = galleryImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteGalleryImagesCommand request, CancellationToken cancellationToken)
        {
            var galleryImagesForDelete = _mapper.Map<IEnumerable<GalleryImage>>(request.galleryImagesForDelete);
            _galleryImageRepository.DeleteMultiple(galleryImagesForDelete);
            await _unitOfWork.SaveAsync();
        }
    }
}
