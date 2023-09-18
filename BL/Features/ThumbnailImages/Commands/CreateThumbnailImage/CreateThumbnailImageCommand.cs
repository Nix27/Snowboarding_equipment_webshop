using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.ThumbnailImages.Commands.CreateThumbnailImage
{
    public record CreateThumbnailImageCommand(IFormFile newThumbnailImage, string title) : IRequest<int>;
}
