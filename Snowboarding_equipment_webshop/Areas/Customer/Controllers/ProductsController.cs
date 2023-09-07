using Microsoft.AspNetCore.Mvc;

namespace Snowboarding_equipment_webshop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        public IActionResult OurProducts()
        {
            return View();
        }
    }
}
