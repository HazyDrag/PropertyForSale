using System;
using System.Collections.Generic;

namespace PropertyForSaleDomainModel.Entities
{
    public class AdType
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        public virtual List<Advert> Adverts { get; set; }
    }
}



