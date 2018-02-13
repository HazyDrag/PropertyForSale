using PropertyForSaleDomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyForSale.Models
{
    public class SearchViewModel
    {
        public IEnumerable<AdvertModel> Adverts { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public AdFilterModel CurrentFilter { get; set; }

        public List<AdTypeModel> Types { get; set; }
    }
}