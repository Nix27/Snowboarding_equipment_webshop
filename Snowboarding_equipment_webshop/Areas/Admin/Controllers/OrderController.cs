using AutoMapper;
using BL.Features.OrderHeaders.Queries.GetAllOrders;
using BL.Features.OrderHeaders.Queries.GetOrderHeaderById;
using BL.Features.OrderHeaders.Queries.GetPagedOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public OrderController(IMediator mediator, IMapper mapper, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> AllOrders(int size, int page, string filterBy)
        {
            if (size == 0)
                size = 5;

            try
            {
                var pagedOrders = await _mediator.Send(new GetPagedOrdersQuery(size, page, filterBy));
                int numberOfAllOrders = _mediator.Send(new GetAllOrdersQuery(includeProperties: "User")).GetAwaiter().GetResult().Count();

                ViewData["size"] = size;
                ViewData["page"] = page;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllOrders / size);

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

        public async Task<IActionResult> OrderTableBodyPartial(int size, int page, string filterBy)
        {
            if (size == 0)
                size = 5;

            try
            {
                var pagedOrders = await _mediator.Send(new GetPagedOrdersQuery(size, page, filterBy));
                int numberOfAllOrders = _mediator.Send(new GetAllOrdersQuery(includeProperties: "User")).GetAwaiter().GetResult().Count();

                ViewData["size"] = size;
                ViewData["page"] = page;
                ViewData["pages"] = (int)Math.Ceiling((double)numberOfAllOrders / size);
                ViewData["action"] = nameof(AllOrders);

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
                var order = await _mediator.Send(new GetOrderHeaderByIdQuery(orderId, includeProperties: "OrderItems.Product"));

                return View(_mapper.Map<OrderVM>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return new StatusCodeResult(500);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> OrderDetails(OrderVM orderVM)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message + "\n" + ex.StackTrace);
        //        TempData["error"] = errorMessage;
        //        return new StatusCodeResult(500);
        //    }
        //}
    }
}
