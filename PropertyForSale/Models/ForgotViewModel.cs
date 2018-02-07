﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyForSale.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public String Email { get; set; }
    }
}