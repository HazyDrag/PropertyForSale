using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyForSale.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public String Email { get; set; }
    }
}
