﻿using System;
using System.Collections.Generic;

namespace PropertyForSaleDomainModel
{
    public class Advert
    {
        public Int32 ID { get; set; }
        public Decimal Price { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String Town { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public String Photo { get; set; }

        
        public ApplicationUser User { get; set; }
        public AdStatus Status { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }


}