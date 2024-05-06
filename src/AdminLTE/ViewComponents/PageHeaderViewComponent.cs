using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.ViewComponents;

public class PageHeaderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string filter)
    {
        Tuple<string, string> message;

        if (ViewBag.PageHeader == null)
        {
            message = Tuple.Create(string.Empty, string.Empty);
        }
        else
        {
            message = ViewBag.PageHeader as Tuple<string, string>;
        }

        return View(message);
    }
}