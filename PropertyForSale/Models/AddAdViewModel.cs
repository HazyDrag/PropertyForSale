using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

using PropertyForSale.Attributes;
using PropertyForSaleDomainModel.Enums;

namespace PropertyForSale.Models
{
    public class AddAdViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be from 3 to 100 characters")]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Town must be from 3 to 50 characters")]
        [Display(Name = "Town")]
        public String Town { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 3, ErrorMessage = "Description must be from 3 to 400 characters")]
        [Display(Name = "Description")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Price is not correct")]
        public Decimal Price { get; set; }

        [Required]
        [Display(Name = "Type")]
        public Int32 TypeID { get; set; }

        public List<AdTypeModel> Types { get; set; }

        [Required]
        [Display(Name = "Status")]
        public AdStatus Status { get; set; }
        
        [Display(Name = "Photo")]
        [ValidateFile(ErrorMessage = "Please select a PNG or JPEG image smaller than 10MB")]
        public HttpPostedFileBase NewPhoto { get; set; }
    }
}