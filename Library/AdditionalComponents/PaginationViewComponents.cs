using Microsoft.AspNetCore.Mvc;

namespace Library.AdditionalComponents
{
    public class PaginationViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
