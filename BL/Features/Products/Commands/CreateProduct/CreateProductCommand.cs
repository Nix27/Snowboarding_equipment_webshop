using BL.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BL.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        ProductDto newProduct, 
        IFormFile? newThumbnailImage = null,
        IEnumerable<IFormFile>? newGalleryImages = null) : IRequest<int>;
}
