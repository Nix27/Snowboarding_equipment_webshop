using BL.Enums;
using BL.Features.GalleryImages.Queries.GetGalleryImageById;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ImageController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ImageController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public ImageController(IMediator mediator, ILogger<ImageController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(ImageType imageType, int id)
        {
            try
            {
                dynamic image = imageType == ImageType.Thumbnail ?
                await _mediator.Send(new GetThumbnailImageByIdQuery(id)) :
                await _mediator.Send(new GetGalleryImageByIdQuery(id));

                var imageContent = image.Content;
                var imageBytes = Convert.FromBase64String(imageContent);

                return new FileContentResult(imageBytes, "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }
    }
}
