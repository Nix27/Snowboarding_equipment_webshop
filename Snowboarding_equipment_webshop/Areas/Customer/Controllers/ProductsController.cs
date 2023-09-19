using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Queries.GetNumberOfProducts;
using BL.Features.Products.Queries.GetPagedProducts;
using BL.Features.Products.Queries.GetProductById;
using BL.Features.ShoppingCartItem.Commands.CreateShoppingCartItem;
using BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem;
using BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemByProductIdAndUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;
using System.Security.Claims;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public ProductsController(IMediator mediator, IMapper mapper, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> OurProducts(PageProductsRequestVM productsRequest)
        {
            if(productsRequest.Size == 0)
                productsRequest.Size = 20;

            try
            {
                var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(null, productsRequest.Page, productsRequest.Size, includeProperties: "Category"));
                int numberOfAllProducts = await _mediator.Send(new GetNumberOfProductsQuery());

                ViewData["page"] = productsRequest.Page;
                ViewData["size"] = productsRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllProducts / productsRequest.Size);

                var pagedProductsVm = _mapper.Map<IEnumerable<ProductVM>>(pagedProducts);

                return View(pagedProductsVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        //need to add a partial view for products

        public async Task<IActionResult> ProductDetails(int productId)
        {
            try
            {
                var product = await _mediator.Send(new GetProductByIdQuery(productId, includeProperties: "Category,GalleryImages"));
                return View(_mapper.Map<ProductVM>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(OurProducts));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddProductItem(ProductVM product, int quantity)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var shoppingCartItemFromDb = await _mediator.Send(new GetShoppingCartItemByProductIdAndUserIdQuery(product.Id, userId));

                if(shoppingCartItemFromDb == null)
                {
                    ShoppingCartItemDto newShoppingCartItem = new()
                    {
                        ProductId = product.Id,
                        UserId = userId,
                        Quantity = quantity
                    };

                    await _mediator.Send(new CreateShoppingCartItemCommand(newShoppingCartItem));
                    //add session
                }
                else
                {
                    await _mediator.Send(new IncrementQuantityOfShoppingCartItemCommand(shoppingCartItemFromDb.Id, quantity));
                }

                return RedirectToAction(nameof(OurProducts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(OurProducts));
            }
        }
    }
}
