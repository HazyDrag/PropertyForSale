using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PropertyForSaleDomainModel.Entities;

namespace PropertyForSale.Models
{
    public class ListViewModel
    {
        public IEnumerable<AdvertModel> Adverts { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}