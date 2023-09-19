using Microsoft.AspNetCore.Mvc;

namespace Snowboarding_equipment_webshop.ViewComponents
{
    public class Search : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View());
        }
    }
}
