namespace AdminLTE.Models;

/// <summary>
///     Defines a module in the sidebar menu in the UI.
/// </summary>
public class SidebarMenu
{
    /// <summary>
    ///     The sidebar menu type, header, link, or tree component.
    /// </summary>
    public SidebarMenuType Type { get; set; }
    
    /// <summary>
    ///     A value to tell if the current element is the active element in the app.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    ///     The name of the menu component.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    ///     An icon class name to show a icon next to the menu element.
    /// </summary>
    public string IconClassName { get; set; }
    
    /// <summary>
    ///     The URL path for where this menu item should link to.
    /// </summary>
    public string URLPath { get; set; }
    
    /// <summary>
    ///     A tree child if the menu contains child elements.
    /// </summary>
    public List<SidebarMenu> TreeChild { get; set; }
    
    /// <summary>
    ///     A counter to tell how menu links is available.
    /// </summary>
    public Tuple<int, int, int> LinkCounter { get; set; }
}

/// <summary>
///     Enums for defining the menu type in the UI.
/// </summary>
/// <remarks>
///     This helps the render of the UI components create them correctly at startup.
/// </remarks>
public enum SidebarMenuType
{
    /// <summary>
    ///     A header component.
    /// </summary>
    Header,
    
    /// <summary>
    ///     A link component.
    /// </summary>
    Link,
    
    /// <summary>
    ///     A tree component.
    /// </summary>
    Tree
}