using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.GalleryImages.Queries.GetGalleryImageById
{
    public record GetGalleryImageByIdQuery(int id) : IRequest<GalleryImageDto?>;
}
