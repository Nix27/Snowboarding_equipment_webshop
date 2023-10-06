using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Queries.GetPagedProductsForCustomer;
using BL.Features.Products.Queries.GetProductById;
using BL.Features.ShoppingCartItem.Commands.CreateShoppingCartItem;
using BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem;
using BL.Features.ShoppingCartItem.Queries.GetNumberOfShoppingCartItemsForUser;
using BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemByProductIdAndUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;
using System.Security.Claims;
using Utilities.Constants.SessionKeys;

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

        public async Task<IActionResult> OurProducts(CustomerProductsPageRequest productsRequest)
        {
            if(productsRequest.Size == 0) productsRequest.Size = 16f;
            if(productsRequest.Page == 0) productsRequest.Page = 1;
            if (productsRequest.Categories == null) productsRequest.Categories = new List<string>() { "all" };

            productsRequest.SortBy = HttpContext.Request.Cookies["ourProductsSortBy"] ?? "none";

            try
            {
                if (!String.IsNullOrEmpty(productsRequest.SearchTerm))
                {
                    HttpContext.Response.Cookies.Append("ourProductsSearchTerm", productsRequest.SearchTerm);

                    string? categoriesAsString = HttpContext.Request.Cookies["ourProductsCategories"];

                    if(categoriesAsString != null)
                        productsRequest.Categories = categoriesAsString.Split(new char[] { ',' }).Select(c => c);
                }
                else
                    HttpContext.Response.Cookies.Delete("ourProductsSearchTerm");
                
                HttpContext.Response.Cookies.Append("ourProductsMinPrice", productsRequest.MinPrice.ToString());
                HttpContext.Response.Cookies.Append("ourProductsMaxPrice", productsRequest.MaxPrice.ToString());
                HttpContext.Response.Cookies.Append("ourProductsSortBy", productsRequest.SortBy);

                string categories = String.Join(",", productsRequest.Categories);
                HttpContext.Response.Cookies.Append("ourProductsCategories", categories);

                (IEnumerable<ProductDto> pagedProducts, int numberOfAllProducts) = await _mediator.Send(new GetPagedProductsForCustomerQuery(productsRequest));

                ViewData["page"] = productsRequest.Page;
                ViewData["size"] = (int)productsRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllProducts / productsRequest.Size);
                ViewData["sortBy"] = productsRequest.SortBy;
                ViewData["minPrice"] = productsRequest.MinPrice;
                ViewData["maxPrice"] = productsRequest.MaxPrice;

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

        public async Task<IActionResult> OurProductsTableBodyPartial(CustomerProductsPageRequest productsRequest)
        {
            if (productsRequest.Size == 0) productsRequest.Size = 16f;
            if (productsRequest.Page == 0) productsRequest.Page = 1;
            if (productsRequest.Categories == null) productsRequest.Categories = new List<string>() { "all" };

            productsRequest.SearchTerm = HttpContext.Request.Cookies["ourProductsSearchTerm"];
            productsRequest.MinPrice = double.Parse(HttpContext.Request.Cookies["ourProductsMinPrice"]);
            productsRequest.MaxPrice = double.Parse(HttpContext.Request.Cookies["ourProductsMaxPrice"]);

            if(productsRequest.SortBy == null)
            {
                productsRequest.SortBy = HttpContext.Request.Cookies["ourProductsSortBy"] ?? "none";
            }
            else
            {
                HttpContext.Response.Cookies.Append("ourProductsSortBy", productsRequest.SortBy);
            }

            string categories = HttpContext.Request.Cookies["ourProductsCategories"] ?? "all";
            productsRequest.Categories = categories.Split(new char[] { ',' }).Select(c => c);

            try
            {
                (IEnumerable<ProductDto> pagedProducts, int numberOfAllProducts) = await _mediator.Send(new GetPagedProductsForCustomerQuery(productsRequest));

                ViewData["page"] = productsRequest.Page;
                ViewData["size"] = (int)productsRequest.Size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllProducts / productsRequest.Size);

                var pagedProductsVm = _mapper.Map<IEnumerable<ProductVM>>(pagedProducts);

                return PartialView("_ProductsPartial", pagedProductsVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction("Index", "Home");
            }
        }

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

                    int numberOfItemsInShoppingCart = await _mediator.Send(new GetNumberOfShoppingCartItemsForUserQuery(userId));

                    HttpContext.Session.SetInt32(SessionKey.ShoppingCart, numberOfItemsInShoppingCart);
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
