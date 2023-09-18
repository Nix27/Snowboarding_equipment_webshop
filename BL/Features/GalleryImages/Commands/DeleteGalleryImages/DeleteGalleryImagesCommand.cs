using BL.DTOs;
using MediatR;

namespace BL.Features.GalleryImages.Commands.DeleteGalleryImages
{
    public record DeleteGalleryImagesCommand(IEnumerable<GalleryImageDto> galleryImagesForDelete) : IRequest;
}
