using AutoMapper;
using BL.DTOs;
using BL.Features.Categories.Queries.GetAllCategories;
using BL.Features.GalleryImages.Commands;
using BL.Features.Products.Commands.CreateProduct;
using BL.Features.Products.Commands.UpdateProduct;
using BL.Features.Products.Queries.GetAllProducts;
using BL.Features.Products.Queries.GetPagedProducts;
using BL.Features.Products.Queries.GetProductById;
using BL.Features.ThumbnailImages.Commands.CreateThumbnailImage;
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

        private const string errorMessage = "Something went wrong. Try again later!";

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> AllProducts(int page, int size, string filterBy, string? searchTerm)
        {
            if (size == 0)
                size = 5;

            var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(page, size, filterBy, searchTerm));
            int? numberOfAllProducts = _mediator.Send(new GetAllProductsQuery()).GetAwaiter().GetResult()?.Count();

            if(pagedProducts != null && numberOfAllProducts != null)
            {
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllProducts / size);

                var pagedProductsVm = _mapper.Map<IEnumerable<ProductVM>>(pagedProducts);

                return View(pagedProductsVm);
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        public async Task<IActionResult> ProductTableBodyPartial(int page, int size, string filterBy, string? searchTerm)
        {
            if (size == 0)
                size = 5;

            var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(page, size, filterBy, searchTerm));
            int? numberOfAllProducts = _mediator.Send(new GetAllProductsQuery()).GetAwaiter().GetResult()?.Count();

            if (pagedProducts != null && numberOfAllProducts != null)
            {
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllProducts / size);

                return PartialView("_ProductTableBodyPartial", _mapper.Map<IEnumerable<ProductVM>>(pagedProducts));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        public async Task<IActionResult> CreateProduct()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductVM newProduct)
        {
            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdThumbnailImageId = await _mediator.Send(new CreateThumbnailImageCommand(newProduct.NewThumbnailImage, newProduct.Name));

                if (createdThumbnailImageId != null)
                    newProduct.ThumbnailImageId = (int)createdThumbnailImageId;

                var createdProduct = await _mediator.Send(new CreateProductCommand(_mapper.Map<ProductDto>(newProduct)));

                var createdGalleryImages = await _mediator.Send(new CreateGalleryImagesCommand(newProduct.GalleryImages, (int)createdProduct, newProduct.Name));

                transaction.Complete();
            }

            TempData["success"] = "Product created successfully";
            return RedirectToAction(nameof(AllProducts));
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {
            var productForUpdate = await _mediator.Send(new GetProductByIdQuery(id));

            if(productForUpdate != null)
            {
                return View(_mapper.Map<ProductVM>(productForUpdate));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }

        public async Task<IActionResult> UpdateProduct(ProductVM productForUpdate)
        {
            if(!ModelState.IsValid) return View(productForUpdate);

            var updatedProduct = await _mediator.Send(new UpdateProductCommand(_mapper.Map<ProductDto>(productForUpdate)));

            if(updatedProduct != null)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(AllProducts));
            }

            TempData["error"] = errorMessage;
            return StatusCode(500);
        }
    }
}
