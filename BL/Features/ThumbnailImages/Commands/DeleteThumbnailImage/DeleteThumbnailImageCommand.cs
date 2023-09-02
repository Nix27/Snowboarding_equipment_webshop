using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage
{
    public record DeleteThumbnailImageCommand(ThumbnailImageDto thumbnailImageForDelete) : IRequest<int>;
}
