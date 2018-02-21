using System.Collections.Generic;

namespace PropertyForSale.Models
{
    public class ListViewModel
    {
        public string MyId { get; set; }

        public IEnumerable<AdvertModel> Adverts { get; set; }

        public PagingInfo PagingInfo { get; set; }        
    }
}