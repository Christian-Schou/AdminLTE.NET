using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string filter)
    {
        return View();
    }
}