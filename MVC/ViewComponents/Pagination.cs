using Microsoft.AspNetCore.Mvc;

namespace MVC.ViewComponents
{
    public class Pagination : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
