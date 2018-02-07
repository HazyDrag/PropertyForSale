using System.Data.Entity.ModelConfiguration;

namespace PropertyForSaleDomainModel
{
    class PhotoMap : EntityTypeConfiguration<Photo>
    {
        public PhotoMap()
        {
            HasKey(p => p.ID);
            HasRequired(p => p.Advert);
            Property(p => p.Path).HasMaxLength(255).IsRequired();
        }
    }
}
