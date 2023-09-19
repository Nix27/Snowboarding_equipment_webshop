using BL.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        ProductDto productForUpdate,
        IFormFile? newThumbnailImage = null,
        IEnumerable<IFormFile>? newGalleryImages = null) : IRequest<int>;
}
