using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyForSaleDomainModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public String UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}