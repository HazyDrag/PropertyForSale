using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyForSaleDomainModel
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public String Email { get; set; }
    }
}