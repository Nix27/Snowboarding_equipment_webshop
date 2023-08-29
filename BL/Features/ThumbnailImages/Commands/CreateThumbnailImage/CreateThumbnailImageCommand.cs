using BL.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Commands.CreateThumbnailImage
{
    public record CreateThumbnailImageCommand(ThumbnailImageDto newThumbnailImage) : IRequest<int?>;
}
