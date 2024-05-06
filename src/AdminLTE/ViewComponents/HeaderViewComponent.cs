using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string filter)
    {
        return View();
    }
}