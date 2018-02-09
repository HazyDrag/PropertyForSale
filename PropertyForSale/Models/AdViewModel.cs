using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyForSale.Models
{
    public class AdViewModel
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public String PhoneNumber { get; set; }

        public String UserName { get; set; }

        public Decimal Price { get; set; }

        public String Type { get; set; }

        public AdStatusModel Status { get; set; }

        public List<PhotoModel> Photos { get; set; }
    }
}