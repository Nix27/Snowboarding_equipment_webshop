using AutoMapper;
using BL.DTOs;
using BL.Features.ShoppingCartItem.Commands.DecrementQuantityOfShoppingCartItem;
using BL.Features.ShoppingCartItem.Commands.DeleteShoppingCartItem;
using BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem;
using BL.Features.ShoppingCartItem.Queries.GetAllShoppingCartItemsForUser;
using BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;
using System.Security.Claims;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IMapper _mapper;

        private const string errorMessage = "Something went wrong. Try again later!";

        public ShoppingCartController(IMediator mediator, ILogger<ShoppingCartController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> ShoppingCart()
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var shoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(claim.Value));

                double totalPrice = 0;

                foreach(var item in shoppingCartItems)
                {
                    totalPrice += item.Product.Price * item.Quantity;
                }

                ViewData["totalPrice"] = totalPrice;

                return View(_mapper.Map<IEnumerable<ShoppingCartItemVM>>(shoppingCartItems));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> IncrementQuantity(int shoppingCartId)
        {
            try
            {
                await _mediator.Send(new IncrementQuantityOfShoppingCartItemCommand(shoppingCartId, 1));
                return RedirectToAction(nameof(ShoppingCart));   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(ShoppingCart));
            }
        }

        public async Task<IActionResult> DecrementQuantity(int shoppingCartId)
        {
            try
            {
                var shoppingCartItem = await _mediator.Send(new GetShoppingCartItemByIdQuery(shoppingCartId, isTracked:false));

                if (shoppingCartItem == null) 
                {
                    _logger.LogError("Shopping cart item not found");
                    TempData["error"] = errorMessage;
                    return RedirectToAction(nameof(ShoppingCart));
                }

                if(shoppingCartItem.Quantity <= 1)
                {
                    await _mediator.Send(new DeleteShoppingCartItemCommand(shoppingCartItem));
                    //add session
                }
                else
                {
                    await _mediator.Send(new DecrementQuantityOfShoppingCartItemCommand(shoppingCartId, 1));
                }

                return RedirectToAction(nameof(ShoppingCart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(ShoppingCart));
            }
        }

        public async Task<IActionResult> DeleteShoppingCartItem(int shoppingCartItemId)
        {
            try
            {
                var shoppingCartItemForDelete = await _mediator.Send(new GetShoppingCartItemByIdQuery(shoppingCartItemId, isTracked:false));

                if(shoppingCartItemForDelete == null)
                {
                    _logger.LogError("Shopping cart item not found.");
                    TempData["error"] = errorMessage;
                    return RedirectToAction(nameof(ShoppingCart));
                }

                await _mediator.Send(new DeleteShoppingCartItemCommand(shoppingCartItemForDelete));
                return RedirectToAction(nameof(ShoppingCart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(ShoppingCart));
            }
        }
    }
}
