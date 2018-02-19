using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PropertyForSale.Attributes
{
    public class PriceAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if ((Decimal)value > (Decimal)9999999999999999.99)
            {
                return false;
            }

            if (Regex.IsMatch(value.ToString(), "[^0-9.,]"))
            {
                return false;
            }

            if (Regex.Replace(value.ToString(), "[0-9]", "").Count() < 2)
            {
                return true;
            }

            return false;
        }
    }
}