using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.ViewComponents;

public class MenuUserViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string filter)
    {
        return View();
    }
}