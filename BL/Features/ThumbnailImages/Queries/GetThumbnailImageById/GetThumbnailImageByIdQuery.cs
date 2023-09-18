using BL.DTOs;
using MediatR;

namespace BL.Features.ThumbnailImages.Queries.GetThumbnailById
{
    public record GetThumbnailImageByIdQuery(int id, bool isTracked = true) : IRequest<ThumbnailImageDto>;
}
