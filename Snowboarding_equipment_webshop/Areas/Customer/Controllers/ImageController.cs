using BL.Enums;
using BL.Features.GalleryImages.Queries.GetGalleryImageById;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ImageController : Controller
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(ImageType imageType, int id)
        {
            dynamic? image = imageType == ImageType.Thumbnail ? 
                await _mediator.Send(new GetThumbnailImageByIdQuery(id)) :
                await _mediator.Send(new GetGalleryImageByIdQuery(id));

            if (image == null)
                return NoContent();

            var imageContent = image.Content;
            var imageBytes = Convert.FromBase64String(imageContent);

            return new FileContentResult(imageBytes, "application/octet-stream");
        }
    }
}
