using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

using PropertyForSale.Attributes;
using PropertyForSale.Enums;

namespace PropertyForSale.Models
{
    public class EditAdViewModel: AddAdViewModel
    {
        public PhotoModel OldPhoto { get; set; }
    }
}