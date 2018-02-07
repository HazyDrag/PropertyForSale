using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyForSaleDomainModel
{
    public class AdvertMap : EntityTypeConfiguration<Advert>
    {
        public AdvertMap()
        {
            //ToTable("Adverts");
            HasKey(a => a.ID);

            //Property(a => a.Username).HasMaxLength(50);
            Property(a => a.Name).HasMaxLength(255);
            //Property(a => a.Name).HasMaxLength(255);
        }
    }
}
