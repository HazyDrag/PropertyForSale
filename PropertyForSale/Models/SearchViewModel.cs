using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropertyForSale.Models
{
    public class SearchViewModel : ListViewModel
    {
        [Display(Name = "Minimal price")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Minimal price is not correct")]
        public Int32? MinPrice { get; set; }

        [Display(Name = "Maximal price")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Maximal price is not correct")]
        public Int32? MaxPrice { get; set; }

        [Display(Name = "Property type")]
        [RegularExpression("[0-9]*$", ErrorMessage = "Type is not correct")]
        public Int32? AdTypeID { get; set; }

        [Display(Name = "Town")]
        public String Town { get; set; }

        public List<AdTypeModel> Types { get; set; }
    }
}