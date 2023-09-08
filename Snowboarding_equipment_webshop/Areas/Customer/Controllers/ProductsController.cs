using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, IMapper mapper, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult OurProducts(PagedProductsRequestVM productsRequest)
        {

            return View();
        }
    }
}
