using System.Collections.Generic;

namespace PropertyForSale.Models
{
    public class ListViewModel
    {
        public IEnumerable<AdvertModel> Adverts { get; set; }

        public PagingInfo PagingInfo { get; set; }        
    }
}