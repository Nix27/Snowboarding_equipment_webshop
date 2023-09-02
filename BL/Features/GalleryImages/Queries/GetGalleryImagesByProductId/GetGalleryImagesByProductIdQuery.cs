using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.GalleryImages.Queries.GetGalleryImagesByProductId
{
    public record GetGalleryImagesByProductIdQuery(int productId, bool isTracked = true) : IRequest<IEnumerable<GalleryImageDto>>;
}
