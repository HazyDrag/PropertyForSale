using System;

namespace PropertyForSaleDomainModel
{
    public class Photo
    {
        public Int32 ID { get; set; }
        public Byte[] Content { get; set; }
        public Advert Advert { get; set; }
    }
}
