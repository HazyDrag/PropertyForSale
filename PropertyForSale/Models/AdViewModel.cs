using System;
using System.Collections.Generic;

using PropertyForSaleDomainModel.Enums;

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

        public AdStatus Status { get; set; }

        public List<PhotoModel> Photos { get; set; }
    }
}