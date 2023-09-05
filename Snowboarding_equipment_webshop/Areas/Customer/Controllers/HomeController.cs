using AutoMapper;
using BL.Features.Products.Queries.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.Models;
using Snowboarding_equipment_webshop.ViewModels;
using System.Diagnostics;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        private const string errorMessage = "Something went wrong. Try again later!";

        public HomeController(IMediator mediator, IMapper mapper, ILogger<HomeController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var bestSellers = _mediator.Send(new GetAllProductsQuery()).GetAwaiter().GetResult().Take(4);
                return View(_mapper.Map<IEnumerable<ProductVM>>(bestSellers).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
                TempData["error"] = errorMessage;
                return StatusCode(500);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}