using System.ComponentModel.DataAnnotations;
using AdminLTE.Models;
using AdminLTE.Models.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.Controllers;

[Authorize(Roles = "SuperAdmins")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<ApplicationUser> userManager;

    public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
    {
        roleManager = roleMgr;
        userManager = userMgr;
    }

    public IActionResult Index()
    {
        return View(roleManager.Roles);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Required] string name)
    {
        if (ModelState.IsValid)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(name));

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrors(result);
        }

        return View(name);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        var members = new List<ApplicationUser>();
        var nonMember = new List<ApplicationUser>();

        foreach (var user in userManager.Users.ToArray())
        {
            var list = await userManager.IsInRoleAsync(user, role.Name)
                ? members
                : nonMember;
            list.Add(user);
        }

        return View(new EditRoleVm
        {
            Role = role,
            Members = members,
            NonMembers = nonMember
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ModifyRoleVm modifyRole)
    {
        IdentityResult result;

        if (ModelState.IsValid)
        {
            var role = await roleManager.FindByIdAsync(modifyRole.RoleId);
            role.Name = modifyRole.RoleName;
            result = await roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                AddErrors(result);
            }

            foreach (var userId in modifyRole.IdsToAdd ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await userManager.AddToRoleAsync(user, modifyRole.RoleName);
                    if (!result.Succeeded)
                    {
                        AddErrors(result);
                    }
                }
            }

            foreach (var userId in modifyRole.IdsToRemove ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await userManager.RemoveFromRoleAsync(user, modifyRole.RoleName);
                    if (!result.Succeeded)
                    {
                        AddErrors(result);
                    }
                }
            }
        }

        if (ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        return View(modifyRole.RoleId);
    }

    public async Task<IActionResult> Delete(string id)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role != null)
        {
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrors(result);
        }
        else
        {
            ModelState.AddModelError("", "No role found");
        }

        return View("Index", roleManager.Roles);
    }
}