using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string filter)
    {
        if (ViewBag.Breadcrumb == null)
        {
            ViewBag.Breadcrumb = new List<Message>();
        }

        return View(ViewBag.Breadcrumb as List<Message>);
    }
}