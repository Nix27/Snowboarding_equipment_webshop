using AutoMapper;
using BL.DTOs;
using BL.Features.Orders.Commands.UpdateOrder;
using BL.Features.Orders.Commands.UpdateOrderStatus;
using BL.Features.Orders.Commands.UpdateSessionIdAndPaymentIntentId;
using BL.Features.Orders.Queries.GetNumberOfOrders;
using BL.Features.Orders.Queries.GetOrderById;
using BL.Features.Orders.Queries.GetPagedOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;
using Stripe;
using Stripe.Checkout;
using Utilities.Constants.Status;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IMediator mediator, IMapper mapper, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllOrders(float size, int page, string filterBy)
        {
            if (size == 0) size = 5f;
            if (page == 0) page = 1;
            if (filterBy == null) filterBy = "None";

            try
            {
                var pagedOrders = await _mediator.Send(new GetPagedOrdersQuery(size, page, filterBy));

                int numberOfAllOrders;

                if (filterBy != "None")
                {
                    numberOfAllOrders = await _mediator.Send(new GetNumberOfOrdersQuery(o => o.OrderStatus == filterBy));
                }
                else
                {
                    numberOfAllOrders = await _mediator.Send(new GetNumberOfOrdersQuery());
                }
                
                HttpContext.Response.Cookies.Append("ordersFilterBy", filterBy);

                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllOrders / size);
                ViewData["filterBy"] = filterBy;

                var pagedOrdersVm = _mapper.Map<IEnumerable<OrderVM>>(pagedOrders);

                return View(pagedOrdersVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> OrderTableBodyPartial(float size, int page, string filterBy)
        {
            try
            {
                filterBy = HttpContext.Request.Cookies["ordersFilterBy"] ?? "None";

                var pagedOrders = await _mediator.Send(new GetPagedOrdersQuery(size, page, filterBy));

                int numberOfAllOrders;

                if (filterBy != "None")
                {
                    numberOfAllOrders = await _mediator.Send(new GetNumberOfOrdersQuery(o => o.OrderStatus == filterBy));
                }
                else
                {
                    numberOfAllOrders = await _mediator.Send(new GetNumberOfOrdersQuery());
                }

                ViewData["page"] = page;
                ViewData["size"] = (int)size;
                ViewData["pages"] = (int)Math.Ceiling(numberOfAllOrders / size);

                var pagedOrdersVm = _mapper.Map<IEnumerable<OrderVM>>(pagedOrders);

                return PartialView("_OrderTableBodyPartial", pagedOrdersVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderByIdQuery(orderId, includeProperties: "OrderItems.Product"));

                OrderVM = _mapper.Map<OrderVM>(order);

                return View(OrderVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderDetails()
        {
            try
            {
                int updatedOrderId = await _mediator.Send(new UpdateOrderCommand(_mapper.Map<OrderDto>(OrderVM)));

                TempData["success"] = "Order updated successfully";

                return RedirectToAction(nameof(OrderDetails), new { orderId = updatedOrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusToProcessing()
        {
            try
            {
                await _mediator.Send(new UpdateOrderStatusCommand(OrderVM.Id, OrderStatuses.StatusInProcess));
                return RedirectToAction(nameof(OrderDetails), new { orderId = OrderVM.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusToShipped()
        {
            try
            {
                var orderFromDb = await _mediator.Send(new GetOrderByIdQuery(OrderVM.Id, isTracked:false));
                if (orderFromDb == null)
                    throw new InvalidOperationException("Order not found");

                orderFromDb.Carrier = OrderVM.Carrier;
                orderFromDb.TrackingNumber = OrderVM.TrackingNumber;
                orderFromDb.OrderStatus = OrderStatuses.StatusShipped;
                orderFromDb.DateOfShipping = DateTime.Now;

                if(orderFromDb.PaymentStatus == PaymentStatuses.PaymentStatusDelayedPayment)
                    orderFromDb.CompanyPaymentDeadline = DateTime.Now.AddDays(30);

                await _mediator.Send(new UpdateOrderCommand(orderFromDb));

                return RedirectToAction(nameof(OrderDetails), new { orderId = OrderVM.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompanyPay()
        {
            try
            {
                var orderFromDb = await _mediator.Send(new GetOrderByIdQuery(OrderVM.Id, isTracked:false, includeProperties:"OrderItems.Product"));
                if (orderFromDb == null)
                    throw new InvalidOperationException("Order not found");

                string successUrl = $"https://localhost:44335/Admin/Order/CompanyOrderConfirmation?orderId={orderFromDb.Id}";
                string cancelUrl = $"https://localhost:44335/Admin/Order/OrderDetails?orderId={orderFromDb.Id}";

                var options = new SessionCreateOptions
                {
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl,
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in orderFromDb.OrderItems)
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

                await _mediator.Send(new UpdateSessionIdAndPaymentIntentIdCommand(orderFromDb.Id, session.Id, session.PaymentIntentId));

                Response.Headers.Add("Location", session.Url);

                return new StatusCodeResult(303);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> CompanyOrderConfirmation(int orderId)
        {
            try
            {
                var orderFromDb = await _mediator.Send(new GetOrderByIdQuery(orderId, isTracked:false));
                if (orderFromDb == null)
                    throw new InvalidOperationException("Order not found");

                if(orderFromDb.PaymentStatus == PaymentStatuses.PaymentStatusDelayedPayment)
                {
                    var sessionService = new SessionService();
                    var session = await sessionService.GetAsync(orderFromDb.SessionId);

                    if(session.PaymentStatus.ToLower() == "paid")
                    {
                        await _mediator.Send(new UpdateSessionIdAndPaymentIntentIdCommand(orderFromDb.Id, session.Id, session.PaymentIntentId));
                        await _mediator.Send(new UpdateOrderStatusCommand(orderFromDb.Id, orderFromDb.OrderStatus, PaymentStatuses.PaymentStatusApproved));
                    }
                }

                TempData["success"] = "Successfully paid";
                return RedirectToAction(nameof(OrderDetails), new { orderId = orderFromDb.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder()
        {
            try
            {
                var orderFromDb = await _mediator.Send(new GetOrderByIdQuery(OrderVM.Id, isTracked:false));
                if (orderFromDb == null)
                    throw new InvalidOperationException("Order not found");

                if(orderFromDb.PaymentStatus == PaymentStatuses.PaymentStatusApproved)
                {
                    var refundOptions = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderFromDb.PaymentIntentId
                    };

                    var refundService = new RefundService();
                    var refund = refundService.Create(refundOptions);

                    await _mediator.Send(new UpdateOrderStatusCommand(orderFromDb.Id, OrderStatuses.StatusCancelled, PaymentStatuses.PaymentStatusRefunded));
                }
                else
                {
                    await _mediator.Send(new UpdateOrderStatusCommand(orderFromDb.Id, OrderStatuses.StatusCancelled, PaymentStatuses.PaymentStatusCancelled));
                }

                return RedirectToAction(nameof(OrderDetails), new { orderId = OrderVM.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }
    }
}
