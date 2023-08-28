using Microsoft.AspNetCore.Mvc;
using Snowboarding_equipment_webshop.ViewModels;

namespace Snowboarding_equipment_webshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public async Task<IActionResult> AllProducts()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductVM newProduct)
        {
            return View();
        }
    }
}
