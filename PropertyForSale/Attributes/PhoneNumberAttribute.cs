using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace PropertyForSale.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                String phoneNumber = Regex.Replace(value.ToString(), @"[-+.#$&)(:/*]", "");
                if (Double.TryParse(phoneNumber, out Double Num))
                {
                    if (phoneNumber.Count() < 17 && phoneNumber.Count() > 9)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}