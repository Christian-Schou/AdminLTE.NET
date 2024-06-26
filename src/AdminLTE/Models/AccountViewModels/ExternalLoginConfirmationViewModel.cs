﻿using System.ComponentModel.DataAnnotations;

namespace AdminLTE.Models.AccountViewModels;

public class ExternalLoginConfirmationViewModel
{
    [Required] [EmailAddress] public string Email { get; set; }
}