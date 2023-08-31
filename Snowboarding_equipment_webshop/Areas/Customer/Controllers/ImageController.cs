using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetImage(int id)
        {
            var thumbnailImage = await _mediator.Send(new GetThumbnailImageByIdQuery(id));
            
            if(thumbnailImage == null) 
                return NoContent();

            var imageContent = thumbnailImage.Content;
            var imageBytes = Convert.FromBase64String(imageContent);

            return new FileContentResult(imageBytes, "application/octet-stream");
        }
    }
}
