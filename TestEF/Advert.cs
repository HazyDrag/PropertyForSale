using System;
using System.Data.Entity;

namespace TestEF
{
    public enum AdStatus { Active, Pause, Stop }

    public class Advert
    {
        public Int32 ID { get; set; }
        public Decimal Price { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String Town { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public String Photo { get; set; }

        // Ссылка на автора
        public User User { get; set; }
        //Ссылка на статус
        public AdStatus Status { get; set; }
    }

    public class AdType
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }

}