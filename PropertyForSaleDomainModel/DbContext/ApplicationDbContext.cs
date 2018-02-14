using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Mapping;

namespace PropertyForSaleDomainModel.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<AdType> AdTypes { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new AdvertMap());
            modelBuilder.Configurations.Add(new PhotoMap());
            modelBuilder.Configurations.Add(new AdTypeMap());
            modelBuilder.Configurations.Add(new ApplicationUserMap());
        }
    }    
}
