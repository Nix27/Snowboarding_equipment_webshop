using MediatR;

namespace BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage
{
    public record DeleteThumbnailImageCommand(int thumbnailImageId) : IRequest<int>;
}
