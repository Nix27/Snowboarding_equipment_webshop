using BL.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ThumbnailImages.Commands.CreateThumbnailImage
{
    public record CreateThumbnailImageCommand(IFormFile newThumbnailImage, string title) : IRequest<int>;
}
