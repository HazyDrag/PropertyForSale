using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PropertyForSaleDomainModel
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<AdType> AdTypes { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public ApplicationDbContext()
            : base(@"Data Source=DESKTOP-9MMJ8SC\SQLEXPRESS;Initial Catalog=PropertyForSale;" +
          "Integrated Security=SSPI", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new AdvertMap());
        }
    }
}
