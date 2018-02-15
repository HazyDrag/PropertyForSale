using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

using PropertyForSale.Attributes;

namespace PropertyForSale.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }
        
        [PhoneNumber(ErrorMessage = "Phone number can contain only digits and symbols .:[]()-+/#$&*\nAnd must be 10 characters long at least and 16 at max")]
        [Display(Name = "Phone number")]
        public String PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        [Display(Name = "Town")]
        public String Town { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public String Email { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}