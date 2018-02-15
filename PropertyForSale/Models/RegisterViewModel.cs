using System;
using System.ComponentModel.DataAnnotations;

using PropertyForSale.Attributes;

namespace PropertyForSale.Models
{

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public String UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public String ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [PhoneNumber(ErrorMessage = "Phone number can contain only digits and symbols .:[]()-+/#$&*\nAnd must be 10 characters long at least and 16 at max")]
        [Display(Name = "Phone number")]
        public String PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public String Email { get; set; }
    }
}