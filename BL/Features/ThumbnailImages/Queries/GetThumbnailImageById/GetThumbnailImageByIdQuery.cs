using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Queries.GetThumbnailById
{
    public record GetThumbnailImageByIdQuery(int id, bool isTracked = true) : IRequest<ThumbnailImageDto>;
}
