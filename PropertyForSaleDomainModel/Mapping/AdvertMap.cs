using System.Data.Entity.ModelConfiguration;

using PropertyForSaleDomainModel.Entities;

namespace PropertyForSaleDomainModel.Mapping
{
    public class AdvertMap : EntityTypeConfiguration<Advert>
    {
        public AdvertMap()
        {
            HasKey(a => a.ID);
            HasRequired(a => a.User);
            Property(a => a.Name).HasMaxLength(100).IsRequired();
            Property(a => a.Town).HasMaxLength(50);
            Property(a => a.Description).HasMaxLength(400);
            Property(a => a.Type).HasMaxLength(40).IsRequired();            
        }
    }
}
