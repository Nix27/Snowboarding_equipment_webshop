using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Queries.GetAllProducts;
using BL.Features.Products.Queries.GetPagedProducts;
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
            {
                productsRequest.Size = 20;
            }

            try
            {
                var pagedProducts = await _mediator.Send(new GetPagedProductsQuery(_mapper.Map<PageProductsRequestDto>(productsRequest)));
                int numberOfAllProducts = _mediator.Send(new GetAllProductsQuery()).GetAwaiter().GetResult().Count();

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
    }
}
