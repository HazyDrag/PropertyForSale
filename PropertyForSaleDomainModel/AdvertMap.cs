using System.Data.Entity.ModelConfiguration;

namespace PropertyForSaleDomainModel
{
    public class AdvertMap : EntityTypeConfiguration<Advert>
    {
        public AdvertMap()
        {
            HasKey(a => a.ID);
            
            Property(a => a.Name).HasMaxLength(100).IsRequired();
            Property(a => a.Town).HasMaxLength(50);
            Property(a => a.Description).HasMaxLength(400);
            Property(a => a.Type).HasMaxLength(40).IsRequired();
            //Property(a => a.User).IsRequired();
        }
    }
}
