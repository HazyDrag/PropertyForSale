using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace TestEF
{
    public class User
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public String Login { get; set; }
        public String Phone { get; set; }
        public String Mail { get; set; }
        public String Town { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Ссылка на объявления
        public virtual List<Advert> Adverts { get; set; }
    }

}
