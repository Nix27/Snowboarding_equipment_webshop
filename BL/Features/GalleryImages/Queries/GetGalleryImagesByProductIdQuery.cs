using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.GalleryImages.Queries
{
    public record GetGalleryImagesByProductIdQuery(int productId) : IRequest<IEnumerable<GalleryImageDto>?>;
}
