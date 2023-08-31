using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.GalleryImages.Commands
{
    public record CreateGalleryImagesCommand(
        IEnumerable<IFormFile> newGalleryImages, 
        int productId, 
        string title) : IRequest<bool>;
}
