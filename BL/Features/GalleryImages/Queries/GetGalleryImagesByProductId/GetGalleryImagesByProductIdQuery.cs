using BL.DTOs;
using MediatR;

namespace BL.Features.GalleryImages.Queries.GetGalleryImagesByProductId
{
    public record GetGalleryImagesByProductIdQuery(int productId, bool isTracked = true) : IRequest<IEnumerable<GalleryImageDto>>;
}
