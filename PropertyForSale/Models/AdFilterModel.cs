using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyForSale.Models
{
    public class AdFilterModel
    {
        [Display(Name = "Property type")]
        public Int32? adTypeID { get; set; }

        [Display(Name = "Maximal price")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Maximal price is not correct")]
        public Int32? maxPrice { get; set; }

        [Display(Name = "Minimal price")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Minimal price is not correct")]
        public Int32? minPrice { get; set; }

        [Display(Name = "Town")]
        public String town { get; set; }
    }
}
