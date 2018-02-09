using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PropertyForSale.Models
{
    public class AddAdViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public Int32 ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Town")]
        public String Town { get; set; }

        [Required]
        [Display(Name = "Description")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        [RegularExpression("[0-9]", ErrorMessage = "Type only numbers")]
        public Decimal Price { get; set; }

        [Required]
        [Display(Name = "Type")]
        public AdTypeModel Type { get; set; }

        [Required]
        [Display(Name = "Status")]
        public AdStatusModel Status { get; set; }
        
        [Display(Name = "Photo")]
        public PhotoModel Photo { get; set; }
    }
}