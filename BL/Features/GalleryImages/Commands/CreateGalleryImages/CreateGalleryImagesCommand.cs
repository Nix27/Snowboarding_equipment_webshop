using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.GalleryImages.Commands.CreateGalleryImages
{
    public record CreateGalleryImagesCommand(
        IEnumerable<IFormFile> newGalleryImages,
        int productId,
        string title) : IRequest;
}
