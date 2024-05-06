using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminLTE.Common.Attributes;

/// <summary>
///     An attribute to use on controller actions to add a help definition on the page.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HelpDefinitionAttribute : ActionFilterAttribute
{
    /// <summary>
    ///     The name of the help file.
    /// </summary>
    private string PageHelpFileName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HelpDefinitionAttribute"/> class.
    /// </summary>
    /// <param name="fileName">The specific filename located in wwwroot\files\Shared folder.</param>
    /// <param name="filePath">The file path of the calling method. This is automatically populated by the runtime.</param>
    /// <param name="memberName">The name of the calling method. This is automatically populated by the runtime.</param>
    public HelpDefinitionAttribute(string fileName = "", [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
    {
        var controllerName = Path.GetFileName(filePath).Replace("Controller.cs", string.Empty);

        // If fileName is not specified, the help file is assumed to be located at a common help location
        // plus the path from class+method.html
        if (string.IsNullOrEmpty(fileName))
        {
            PageHelpFileName = Path.Combine(controllerName, memberName); // Format: ChildController\ActionMethod
        }
        else // If fileName is specified, the help file is assumed to be located in the common help location
        {
            PageHelpFileName = Path.Combine("Shared", fileName);
        }
    }

    /// <summary>
    ///     Called by the ASP.NET Core framework before the action result executes.
    ///     It adds the help file name to the ViewBag.
    /// </summary>
    /// <param name="filterContext">The context for the action filter.</param>
    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        if (filterContext.Controller is Controller controller)
        {
            controller.ViewBag.PageHelpFileName = PageHelpFileName;
        }
    }
}