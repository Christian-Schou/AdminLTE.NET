using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.Controllers;

public class BaseController : Controller
{
    internal void AddBreadcrumb(string displayName, string urlPath)
    {
        List<Message> messages;

        if (ViewBag.Breadcrumb == null)
        {
            messages = new List<Message>();
        }
        else
        {
            messages = ViewBag.Breadcrumb as List<Message>;
        }

        messages.Add(new Message { DisplayName = displayName, URLPath = urlPath });
        ViewBag.Breadcrumb = messages;
    }

    internal void AddPageHeader(string pageHeader = "", string pageDescription = "")
    {
        ViewBag.PageHeader = Tuple.Create(pageHeader, pageDescription);
    }

    internal void AddPageAlerts(PageAlertType pageAlertType, string description)
    {
        List<Message> messages;

        if (ViewBag.PageAlerts == null)
        {
            messages = new List<Message>();
        }
        else
        {
            messages = ViewBag.PageAlerts as List<Message>;
        }

        messages.Add(new Message { Type = pageAlertType.ToString().ToLower(), ShortDesc = description });
        ViewBag.PageAlerts = messages;
    }

    internal enum PageAlertType
    {
        Error,
        Info,
        Warning,
        Success
    }
}