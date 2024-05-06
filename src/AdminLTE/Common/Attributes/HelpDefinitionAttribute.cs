using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminLTE.Common.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class HelpDefinitionAttribute : ActionFilterAttribute, IActionFilter
{
    /// <summary>
    ///     By declaring this on the IActionResult method, you are enabling
    /// </summary>
    /// <param name="fileName">specify specific filename located in wwwroot\files\Shared folder</param>
    /// <param name="filePath">THIS MUST BE EMPTY</param>
    /// <param name="memberName">THIS MUST BE EMPTY</param>
    public HelpDefinitionAttribute(string fileName = "", [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "")
    {
        var controllerName = filePath.Split('\\').Last().Replace("Controller.cs", string.Empty);
        if (fileName ==
            string.Empty) //if not specified, will check it on the common help location plus path from class+method.html
        {
            _pageHelpFileName = controllerName + @"\" + memberName; //ChildController + ActionMethod
        }
        else //if specified, will check it in the common help location
        {
            _pageHelpFileName = @"Shared\" + fileName;
        }
    }

    private string _fileName { get; set; }
    private string _memberName { get; set; }
    private string _pageHelpFileName { get; }


    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        var controller = filterContext.Controller as Controller;
        controller.ViewBag.PageHelpFileName = _pageHelpFileName;
    }
}