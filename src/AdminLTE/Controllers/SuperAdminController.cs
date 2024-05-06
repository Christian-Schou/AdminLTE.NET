using AdminLTE.Models;
using AdminLTE.Models.SuperAdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.Controllers;

[Authorize(Roles = "SuperAdmins")]
public class SuperAdminController : Controller
{
    private readonly IPasswordHasher<ApplicationUser> passwordHasher;
    private readonly IPasswordValidator<ApplicationUser> passwordValidator;

    private readonly ApplicationUser testUser = new()
    {
        UserName = "TestTestForPassword",
        Email = "testForPassword@test.test"
    };

    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUserValidator<ApplicationUser> userValidator;

    public SuperAdminController(UserManager<ApplicationUser> userMgr,
        IUserValidator<ApplicationUser> userValid, IPasswordValidator<ApplicationUser> passValid,
        IPasswordHasher<ApplicationUser> passHasher)
    {
        userManager = userMgr;
        userValidator = userValid;
        passwordValidator = passValid;
        passwordHasher = passHasher;
    }

    // GET: /<controller>/
    public ViewResult Index()
    {
        return View(userManager.Users);
    }

    public ViewResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateVm createVm)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = createVm.Email,
                Email = createVm.Email,
                //extended properties
                FirstName = createVm.FirstName,
                LastName = createVm.LastName,
                AvatarURL = "/images/user.png",
                DateRegistered = DateTime.UtcNow.ToString(),
                Position = "",
                NickName = ""
            };

            var result = await userManager.CreateAsync(user, createVm.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(createVm);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.TryAddModelError("", error.Description);
        }
    }

    public async Task<IActionResult> Delete(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user != null)
        {
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrors(result);
        }
        else
        {
            ModelState.AddModelError("", "User Not Found");
        }

        return View("Index", userManager.Users);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user != null)
        {
            return View(user);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // the names of its parameters must be the same as the property of the User class if we use asp-for in the view
    // otherwise form values won't be passed properly
    public async Task<IActionResult> Edit(string id, string userName, string email)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user != null)
        {
            // Validate UserName and Email 
            user.UserName =
                userName; // UserName won't be changed in the database until UpdateAsync is executed successfully
            user.Email = email;
            var validUseResult = await userValidator.ValidateAsync(userManager, user);
            if (!validUseResult.Succeeded)
            {
                AddErrors(validUseResult);
            }

            // Update user info
            if (validUseResult.Succeeded)
            {
                // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "SuperAdmin");
                }

                AddErrors(result);
            }
        }
        else
        {
            ModelState.AddModelError("", "User Not Found");
        }

        ;

        return View(user);
    }

    public async Task<IActionResult> ChangePassword(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user != null)
        {
            return View(user);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string id, string password)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user != null)
        {
            // Validate password
            // Step 1: using built in validations
            var passwordResult = await userManager.CreateAsync(testUser, password);
            if (passwordResult.Succeeded)
            {
                await userManager.DeleteAsync(testUser);
            }
            else
            {
                AddErrors(passwordResult);
            }

            /* Step 2: Because of DI, IPasswordValidator<User> is injected into the custom password validator.
               So the built in password validation stop working here */
            var validPasswordResult = await passwordValidator.ValidateAsync(userManager, user, password);
            if (validPasswordResult.Succeeded)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, password);
            }
            else
            {
                AddErrors(validPasswordResult);
            }

            // Update user info
            if (passwordResult.Succeeded && validPasswordResult.Succeeded)
            {
                // UpdateAsync validates user info such as UserName and Email except password since it's been hashed 
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "SuperAdmin");
                }

                AddErrors(result);
            }
        }
        else
        {
            ModelState.AddModelError("", "User Not Found");
        }

        return View(user);
    }
}