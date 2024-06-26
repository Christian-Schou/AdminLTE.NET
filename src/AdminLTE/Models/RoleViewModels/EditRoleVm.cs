﻿using Microsoft.AspNetCore.Identity;

namespace AdminLTE.Models.RoleViewModels;

public class EditRoleVm
{
    public IdentityRole Role { get; set; }
    public IEnumerable<ApplicationUser> Members { get; set; }
    public IEnumerable<ApplicationUser> NonMembers { get; set; }
}