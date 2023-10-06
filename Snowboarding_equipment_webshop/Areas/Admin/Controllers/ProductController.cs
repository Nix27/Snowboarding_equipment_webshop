using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Queries.GetAllCategories;
using BL.Features.Products.Commands.CreateProduct;
using BL.Features.Products.Commands.DeleteProduct;
using BL.Features.Products.Commands.UpdateProduct;
using BL.Features.Products.Queries.GetNumberOfProducts;
using BL.Features.Products.Queries.GetPagedProducts;
using BL.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snowboarding_equipment_webshop.ViewModels;
using Utilities.Constants.Role;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.ADMIN)]
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

        public async Task<IActionResult> AllProducts(PageProductsRequest pageRequest)
        {
            if (pageRequest.Size == 0) pageRequest.Size = 5f;
            if(pageRequest.Page == 0) pageRequest.Page = 1;

            int numberOfAllProducts;

            try
            {
                if (!String.IsNullOrEmpty(pageRequest.SearchTerm))
                {
                    numberOfAllProducts = await _mediator.Send(new GetNumberOfProductsQuery(p => p.Name.Contains(pageRequest.SearchTerm)));
                    HttpContext.Response.Cookies.Append("productSearchTerm", pageRequest.SearchTerm);
                    HttpContext.Response.Cookies.Append("productsSearchBy", pageRequest.SearchBy);
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("productSearchTerm");
                    numberOfAllProducts = await _mediator.Send(new GetNumberOfProductsQuery());
                }

                var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(pageRequest, includeProperties: "Category"));

                ViewData["page"] = pageRequest.Page;
                ViewData["size"] = (int)pageRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllProducts / pageRequest.Size);
                ViewData["searchBy"] = pageRequest.SearchBy;

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

        public async Task<IActionResult> ProductTableBodyPartial(PageProductsRequest pageRequest)
        {
            int numberOfAllProducts;

            pageRequest.SearchTerm = HttpContext.Request.Cookies["productSearchTerm"];
            pageRequest.SearchBy = HttpContext.Request.Cookies["productsSearchBy"];

            try
            {
                if (!String.IsNullOrEmpty(pageRequest.SearchTerm))
                {
                    numberOfAllProducts = await _mediator.Send(new GetNumberOfProductsQuery(p => p.Name.Contains(pageRequest.SearchTerm)));
                }
                else
                {
                    numberOfAllProducts = await _mediator.Send(new GetNumberOfProductsQuery());
                }

                var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(pageRequest, includeProperties: "Category"));

                ViewData["page"] = pageRequest.Page;
                ViewData["size"] = (int)pageRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllProducts / pageRequest.Size);

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

                var newProductDto = _mapper.Map<ProductDto>(newProduct);

                await _mediator.Send(new CreateProductCommand(newProductDto, newProduct.NewThumbnailImage, newProduct.NewGalleryImages));

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
                var productForUpdate = await _mediator.Send(new GetProductByIdQuery(id, includeProperties:"Category,GalleryImages"));
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

                var productForUpdateDto = _mapper.Map<ProductDto>(productForUpdate);
                await _mediator.Send(new UpdateProductCommand(productForUpdateDto, productForUpdate.NewThumbnailImage, productForUpdate.NewGalleryImages));

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
                var deletedProduct = await _mediator.Send(new DeleteProductCommand(id));

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
