using System.Data.Entity.ModelConfiguration;

using PropertyForSaleDomainModel.Entities;

namespace PropertyForSaleDomainModel.Mapping
{
    class ApplicationUserMap : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            ToTable("AspNetUsers");

            HasKey(a => a.Id);

            Property(a => a.Town).HasMaxLength(50).IsOptional();
            Property(a => a.Email).HasMaxLength(100).IsRequired();
            Property(a => a.PasswordHash).IsRequired();
            Property(a => a.PhoneNumber).HasMaxLength(16).IsRequired();
            Property(a => a.UserName).HasMaxLength(31).IsRequired();
            Property(a => a.Name).HasMaxLength(40).IsRequired();
        }
    }
}