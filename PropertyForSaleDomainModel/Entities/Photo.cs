﻿using System;

namespace PropertyForSaleDomainModel.Entities
{
    public class Photo
    {
        public Int32 ID { get; set; }
        public String Path { get; set; }
        
        public virtual Advert Advert { get; set; }
    }
}