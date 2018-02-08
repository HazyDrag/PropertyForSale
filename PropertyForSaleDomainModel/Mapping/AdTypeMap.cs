using System.Data.Entity.ModelConfiguration;

using PropertyForSaleDomainModel.Entities;

namespace PropertyForSaleDomainModel.Mapping
{
    class AdTypeMap : EntityTypeConfiguration<AdType>
    {
        public AdTypeMap()
        {
            HasKey(a => a.ID);

            Property(a => a.Name).HasMaxLength(50).IsRequired();
            Property(a => a.Description).HasMaxLength(255);
        }
    }
}