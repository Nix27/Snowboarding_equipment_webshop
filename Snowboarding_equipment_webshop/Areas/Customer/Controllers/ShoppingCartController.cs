using AutoMapper;
using BL.DTOs;
using BL.Features.OrderDetails.Commands.CreateOrderDetail;
using BL.Features.OrderHeaders.Commands.CreateOrderHeader;
using BL.Features.OrderHeaders.Commands.UpdateOrderStatus;
using BL.Features.OrderHeaders.Commands.UpdateSessionIdAndPaymentIntentId;
using BL.Features.OrderHeaders.Queries.GetOrderHeaderById;
using BL.Features.ShoppingCartItem.Commands.DecrementQuantityOfShoppingCartItem;
using BL.Features.ShoppingCartItem.Commands.DeleteMultipleShoppingCartItems;
using BL.Features.ShoppingCartItem.Commands.DeleteShoppingCartItem;
using BL.Features.ShoppingCartItem.Commands.IncrementQuantityOfShoppingCartItem;
using BL.Features.ShoppingCartItem.Queries.GetAllShoppingCartItemsForUser;
using BL.Features.ShoppingCartItem.Queries.GetShoppingCartItemById;
using BL.Features.Users.Queries.GetUserById;
using DAL.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;
using Utilities.Constants.Role;
using Utilities.Constants.Status;

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
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var shoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(userId));

                ShoppingCartVM shoppingCartVM = new()
                {
                    ShoppingCartItems = shoppingCartItems,
                    OrderHeader = new()
                };

                foreach (var item in shoppingCartItems)
                {
                    shoppingCartVM.OrderHeader.TotalPrice += item.Product.Price * item.Quantity;
                }

                return View(shoppingCartVM);
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
                var shoppingCartItem = await _mediator.Send(new GetShoppingCartItemByIdQuery(shoppingCartId, isTracked: false));

                if (shoppingCartItem == null)
                {
                    _logger.LogError("Shopping cart item not found");
                    TempData["error"] = errorMessage;
                    return RedirectToAction(nameof(ShoppingCart));
                }

                if (shoppingCartItem.Quantity <= 1)
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
                var shoppingCartItemForDelete = await _mediator.Send(new GetShoppingCartItemByIdQuery(shoppingCartItemId, isTracked: false));

                if (shoppingCartItemForDelete == null)
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

        public async Task<IActionResult> OrderSummary()
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var shoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(userId));

                ShoppingCartVM shoppingCartVM = new()
                {
                    ShoppingCartItems = shoppingCartItems,
                    OrderHeader = new()
                };

                var user = await _mediator.Send(new GetUserByIdQuery(userId));
                shoppingCartVM.OrderHeader.User = _mapper.Map<User>(user);

                shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.User.Name;
                shoppingCartVM.OrderHeader.Phone = shoppingCartVM.OrderHeader.User.Phone;
                shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.User.StreetAddress;
                shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.User.City;
                shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.User.PostalCode;
                shoppingCartVM.OrderHeader.Country = shoppingCartVM.OrderHeader.User.Country.Name;

                foreach (var item in shoppingCartItems)
                {
                    shoppingCartVM.OrderHeader.TotalPrice += item.Product.Price * item.Quantity;
                }

                return View(shoppingCartVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(ShoppingCart));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> OrderSummary(ShoppingCartVM shoppingCartVM)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                shoppingCartVM.ShoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(userId));
                shoppingCartVM.OrderHeader.DateOfOrder = DateTime.Now;
                shoppingCartVM.OrderHeader.UserId = userId;

                if (!User.IsInRole(AppRoles.COMPANY))
                {
                    shoppingCartVM.OrderHeader.OrderStatus = OrderStatuses.StatusPending;
                    shoppingCartVM.OrderHeader.PaymentStatus = PaymentStatuses.PaymentStatusPending;
                }
                else
                {
                    shoppingCartVM.OrderHeader.OrderStatus = OrderStatuses.StatusApproved;
                    shoppingCartVM.OrderHeader.PaymentStatus = PaymentStatuses.PaymentStatusDelayedPayment;
                }

                int orderHeaderId = await _mediator.Send(new CreateOrderHeaderCommand(shoppingCartVM.OrderHeader));

                foreach (var item in shoppingCartVM.ShoppingCartItems)
                {
                    OrderDetailDto newOrderDetail = new()
                    {
                        ProductId = item.ProductId,
                        OrderHeaderId = orderHeaderId,
                        Price = item.Product.Price,
                        Quantity = item.Quantity
                    };

                    await _mediator.Send(new CreateOrderDetailCommand(newOrderDetail));
                }

                if (!User.IsInRole(AppRoles.COMPANY))
                {
                    string successUrl = $"https://localhost:44335/Customer/ShoppingCart/OrderConfirmation?orderHeaderId={orderHeaderId}";
                    const string cancelUrl = "https://localhost:44335/Customer/ShoppingCart/OrderSummary";

                    var options = new SessionCreateOptions
                    {
                        SuccessUrl = successUrl,
                        CancelUrl = cancelUrl,
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                    };

                    foreach(var item in shoppingCartVM.ShoppingCartItems)
                    {
                        var sessionLineItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(item.Product.Price * 100),
                                Currency = "eur",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name
                                }
                            },
                            Quantity = item.Quantity
                        };
                        
                        options.LineItems.Add(sessionLineItem);
                    }

                    var service = new SessionService();
                    var session = service.Create(options);

                    await _mediator.Send(new UpdateSessionIdAndPaymentIntentIdCommand(orderHeaderId, session.Id, session.PaymentIntentId));

                    Response.Headers.Add("Location", session.Url);

                    return new StatusCodeResult(303);
                }

                return RedirectToAction(nameof(OrderConfirmation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return RedirectToAction(nameof(ShoppingCart));
            }
        }

        public async Task<IActionResult> OrderConfirmation(int orderHeaderId)
        {
            try
            {
                var orderHeader = await _mediator.Send(new GetOrderHeaderByIdQuery(orderHeaderId));

                if (orderHeader == null)
                    throw new InvalidOperationException("OrderHeader not found.");

                if (orderHeader.PaymentStatus != PaymentStatuses.PaymentStatusDelayedPayment)
                {
                    var service = new SessionService();
                    var session = service.Get(orderHeader.SessionId);

                    if(session.PaymentStatus.ToLower() == "paid")
                    {
                        await _mediator.Send(new UpdateSessionIdAndPaymentIntentIdCommand(orderHeader.Id, session.Id, session.PaymentIntentId));
                        await _mediator.Send(new UpdateOrderStatusCommand(orderHeader.Id, OrderStatuses.StatusApproved, PaymentStatuses.PaymentStatusApproved));
                    }
                }

                var shoppingCartItemsForDelete = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(orderHeader.UserId, isTracked:false));
                await _mediator.Send(new DeleteMultipleShoppingCartItemsCommand(shoppingCartItemsForDelete));

                return View();
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
