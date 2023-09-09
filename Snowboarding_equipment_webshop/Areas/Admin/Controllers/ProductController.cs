using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Queries.GetAllCategories;
using BL.Features.GalleryImages.Commands.CreateGalleryImages;
using BL.Features.GalleryImages.Commands.DeleteGalleryImages;
using BL.Features.GalleryImages.Queries.GetGalleryImagesByProductId;
using BL.Features.Products.Commands.CreateProduct;
using BL.Features.Products.Commands.DeleteProduct;
using BL.Features.Products.Commands.UpdateProduct;
using BL.Features.Products.Queries.GetAllProducts;
using BL.Features.Products.Queries.GetPagedProducts;
using BL.Features.Products.Queries.GetProductById;
using BL.Features.ThumbnailImages.Commands.CreateThumbnailImage;
using BL.Features.ThumbnailImages.Commands.DeleteThumbnailImage;
using BL.Features.ThumbnailImages.Queries.GetThumbnailById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snowboarding_equipment_webshop.ViewModels;
using System.Transactions;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public ProductController(IMediator mediator, IMapper mapper, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllProducts(PageProductsRequestVM pageRequest)
        {
            if (pageRequest.Size == 0)
                pageRequest.Size = 5;

            try
            {
                var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(_mapper.Map<PageProductsRequestDto>(pageRequest)));
                int numberOfAllProducts = _mediator.Send(new GetAllProductsQuery()).GetAwaiter().GetResult().Count();

                ViewData["page"] = pageRequest.Page;
                ViewData["size"] = pageRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllProducts / pageRequest.Size);

                var pagedProductsVm = _mapper.Map<IEnumerable<ProductVM>>(pagedProducts);

                return View(pagedProductsVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> ProductTableBodyPartial(PageProductsRequestVM pageRequest)
        {
            if (pageRequest.Size == 0)
                pageRequest.Size = 5;

            try
            {
                var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(_mapper.Map<PageProductsRequestDto>(pageRequest)));
                int numberOfAllProducts = _mediator.Send(new GetAllProductsQuery()).GetAwaiter().GetResult().Count();

                ViewData["page"] = pageRequest.Page;
                ViewData["size"] = pageRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllProducts / pageRequest.Size);
                ViewData["action"] = nameof(AllProducts);

                return PartialView("_ProductTableBodyPartial", _mapper.Map<IEnumerable<ProductVM>>(pagedProducts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                var allCategories = await _mediator.Send(new GetAllCategoriesQuery());

                ProductVM productVm = new()
                {
                    Categories = allCategories.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };

                return View(productVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return NoContent();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductVM newProduct)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var allCategories = await _mediator.Send(new GetAllCategoriesQuery());

                    newProduct.Categories = allCategories.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    });

                    return View(newProduct);
                }

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if(newProduct.NewThumbnailImage != null)
                    {
                        var createdThumbnailImageId = await _mediator.Send(new CreateThumbnailImageCommand(newProduct.NewThumbnailImage, newProduct.Name));
                        newProduct.ThumbnailImageId = createdThumbnailImageId;
                    }

                    var createdProductId = await _mediator.Send(new CreateProductCommand(_mapper.Map<ProductDto>(newProduct)));

                    if(newProduct.NewGalleryImages != null)
                    {
                        await _mediator.Send(new CreateGalleryImagesCommand(newProduct.NewGalleryImages, createdProductId, newProduct.Name));
                    }
                    
                    transaction.Complete();
                }

                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(AllProducts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(AllProducts));
            }
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {
            try
            {
                var productForUpdate = await _mediator.Send(new GetProductByIdQuery(id));
                var allCategories = await _mediator.Send(new GetAllCategoriesQuery());

                var productVM = _mapper.Map<ProductVM>(productForUpdate);

                productVM.Categories = allCategories.Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return View(productVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return NoContent();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(ProductVM productForUpdate)
        {
            try
            {
                if (!ModelState.IsValid) return View(productForUpdate);

                var oldThumbnailImageId = productForUpdate.ThumbnailImageId;

                using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (productForUpdate.NewThumbnailImage != null)
                    {
                        var updatedThumbnailImageId = await _mediator.Send(new CreateThumbnailImageCommand(productForUpdate.NewThumbnailImage, productForUpdate.Name));
                        productForUpdate.ThumbnailImageId = updatedThumbnailImageId;
                    }

                    var updatedProductId = await _mediator.Send(new UpdateProductCommand(_mapper.Map<ProductDto>(productForUpdate)));

                    if (oldThumbnailImageId != null && productForUpdate.NewThumbnailImage != null)
                    {
                        var thumbnailImageForDelete = await _mediator.Send(new GetThumbnailImageByIdQuery((int)oldThumbnailImageId, false));
                        var deletedThumbnailImage = await _mediator.Send(new DeleteThumbnailImageCommand(thumbnailImageForDelete));
                    }

                    if (productForUpdate.NewGalleryImages != null)
                    {
                        var galleryImagesForDelete = await _mediator.Send(new GetGalleryImagesByProductIdQuery(updatedProductId, false));
                        await _mediator.Send(new DeleteGalleryImagesCommand(galleryImagesForDelete));
                        await _mediator.Send(new CreateGalleryImagesCommand(productForUpdate.NewGalleryImages, updatedProductId, productForUpdate.Name));
                    }

                    transaction.Complete();
                }

                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(AllProducts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(AllProducts));
            }
        }

        #region API Calls
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var productForDelete = await _mediator.Send(new GetProductByIdQuery(id, false));

                if (productForDelete == null)
                    return Json(new { success = false, message = "Product not found." });

                var deletedProduct = await _mediator.Send(new DeleteProductCommand(productForDelete));

                return Json(new { success = true, message = "Successfully deleted product" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                return Json(new { success = false, message = errorMessage });
            }
        }
        #endregion
    }
}
