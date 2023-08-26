using Microsoft.AspNetCore.Mvc;

namespace Snowboarding_equipment_webshop.ViewComponents
{
    public class Search : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
