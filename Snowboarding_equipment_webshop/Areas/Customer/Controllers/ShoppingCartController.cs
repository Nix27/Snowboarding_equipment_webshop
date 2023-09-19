using AutoMapper;
using BL.DTOs;
using BL.Features.OrderItems.Commands.CreateOrderItem;
using BL.Features.Orders.Commands.CreateOrder;
using BL.Features.Orders.Commands.UpdateOrderStatus;
using BL.Features.Orders.Commands.UpdateSessionIdAndPaymentIntentId;
using BL.Features.Orders.Queries.GetOrderById;
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
using System.Transactions;
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

                var shoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(userId, includeProperties:"Product"));

                ShoppingCartVM shoppingCartVM = new()
                {
                    ShoppingCartItems = shoppingCartItems,
                    Order = new()
                };

                foreach (var item in shoppingCartItems)
                {
                    shoppingCartVM.Order.TotalPrice += item.Product.Price * item.Quantity;
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
                    throw new InvalidOperationException("Shopping cart item not found");

                if (shoppingCartItem.Quantity <= 1)
                {
                    await _mediator.Send(new DeleteShoppingCartItemCommand(shoppingCartItem.Id));
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
                await _mediator.Send(new DeleteShoppingCartItemCommand(shoppingCartItemId));
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

                var shoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(userId, includeProperties: "Product"));

                ShoppingCartVM shoppingCartVM = new()
                {
                    ShoppingCartItems = shoppingCartItems,
                    Order = new()
                };

                var user = await _mediator.Send(new GetUserByIdQuery(userId));
                shoppingCartVM.Order.User = _mapper.Map<User>(user);

                shoppingCartVM.Order.Name = shoppingCartVM.Order.User.Name;
                shoppingCartVM.Order.Phone = shoppingCartVM.Order.User.Phone;
                shoppingCartVM.Order.StreetAddress = shoppingCartVM.Order.User.StreetAddress;
                shoppingCartVM.Order.City = shoppingCartVM.Order.User.City;
                shoppingCartVM.Order.PostalCode = shoppingCartVM.Order.User.PostalCode;
                shoppingCartVM.Order.Country = shoppingCartVM.Order.User.Country.Name;

                foreach (var item in shoppingCartItems)
                {
                    shoppingCartVM.Order.TotalPrice += item.Product.Price * item.Quantity;
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

                shoppingCartVM.ShoppingCartItems = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(userId, includeProperties:"Product"));
                shoppingCartVM.Order.DateOfOrder = DateTime.Now;
                shoppingCartVM.Order.UserId = userId;

                if (!User.IsInRole(AppRoles.COMPANY))
                {
                    shoppingCartVM.Order.OrderStatus = OrderStatuses.StatusPending;
                    shoppingCartVM.Order.PaymentStatus = PaymentStatuses.PaymentStatusPending;
                }
                else
                {
                    shoppingCartVM.Order.OrderStatus = OrderStatuses.StatusApproved;
                    shoppingCartVM.Order.PaymentStatus = PaymentStatuses.PaymentStatusDelayedPayment;
                }

                int orderId = 0;

                using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    orderId = await _mediator.Send(new CreateOrderCommand(shoppingCartVM.Order));

                    foreach (var item in shoppingCartVM.ShoppingCartItems)
                    {
                        OrderItemDto newOrderDetail = new()
                        {
                            ProductId = item.ProductId,
                            OrderId = orderId,
                            Price = item.Product.Price,
                            Quantity = item.Quantity
                        };

                        await _mediator.Send(new CreateOrderItemCommand(newOrderDetail));
                    }

                    transaction.Complete();
                }
                

                if (!User.IsInRole(AppRoles.COMPANY))
                {
                    string successUrl = $"https://localhost:44335/Customer/ShoppingCart/OrderConfirmation?orderId={orderId}";
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

                    await _mediator.Send(new UpdateSessionIdAndPaymentIntentIdCommand(orderId, session.Id, session.PaymentIntentId));

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

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderByIdQuery(orderId));

                if (order.PaymentStatus != PaymentStatuses.PaymentStatusDelayedPayment)
                {
                    var service = new SessionService();
                    var session = service.Get(order.SessionId);

                    if(session.PaymentStatus.ToLower() == "paid")
                    {
                        await _mediator.Send(new UpdateSessionIdAndPaymentIntentIdCommand(order.Id, session.Id, session.PaymentIntentId));
                        await _mediator.Send(new UpdateOrderStatusCommand(order.Id, OrderStatuses.StatusApproved, PaymentStatuses.PaymentStatusApproved));
                    }
                }

                var shoppingCartItemsForDelete = await _mediator.Send(new GetAllShoppingCartItemsForUserQuery(order.UserId, isTracked:false));
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
