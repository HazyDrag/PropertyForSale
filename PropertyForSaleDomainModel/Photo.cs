using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyForSaleDomainModel
{
    public class Photo
    {
        public Int32 ID { get; set; }
        public String Path { get; set; }
        
        public Advert Advert { get; set; }
    }
}
