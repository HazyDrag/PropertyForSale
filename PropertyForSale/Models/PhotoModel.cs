using System;

namespace PropertyForSale.Models
{
    public class PhotoModel
    {
        public Int32 ID { get; set; }
        public String Path { get; set; }
        
        public AdvertModel Advert { get; set; }
    }
}
