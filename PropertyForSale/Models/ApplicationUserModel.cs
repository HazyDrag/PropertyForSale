using System;
using System.Collections.Generic;

namespace PropertyForSale.Models
{
    public class ApplicationUserModel
    {
        public String ID { get; set; }

        public String Name { get; set; }

        public String PhoneNumber { get; set; }

        public String Town { get; set; }

        public String Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<AdvertModel> Adverts { get; set; }
    }
}

