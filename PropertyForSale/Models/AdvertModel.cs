﻿using System;
using System.Collections.Generic;

namespace PropertyForSale.Models
{
    public class AdvertModel
    {
        public Int32 ID { get; set; }
        public Decimal Price { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String Town { get; set; }
        public String Description { get; set; }

        public AdStatusModel Status { get; set; }
        public ApplicationUserModel User { get; set; }
        public AdTypeModel AdType { get; set; }

        public List<PhotoModel> Photos { get; set; }
    }
}