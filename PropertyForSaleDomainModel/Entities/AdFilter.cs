using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyForSaleDomainModel.Entities
{
    public class AdFilter
    {
        public Int32? minPrice { get; set; }

        public Int32? maxPrice { get; set; }

        public String town { get; set; }

        public AdType adType { get; set; }
    }
}
