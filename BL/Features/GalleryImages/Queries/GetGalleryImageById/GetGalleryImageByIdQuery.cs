using BL.DTOs;
using MediatR;

namespace BL.Features.GalleryImages.Queries.GetGalleryImageById
{
    public record GetGalleryImageByIdQuery(int id, bool isTracked = true) : IRequest<GalleryImageDto>;
}
