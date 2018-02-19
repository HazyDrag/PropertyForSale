using System;
using System.Collections.Generic;
using System.Linq;

using PropertyForSale.Enums;
using PropertyForSaleDomainModel.DbContext;
using PropertyForSaleDomainModel.Entities;

namespace PropertyForSaleDomainModel.Repository
{
    public class AdvertRepository : IRepository
    {

        public Int32 SaveAdvert(Advert ad)
        {
            if (ad.ID == 0)
            {
                ad.User = Context.Users
                    .Where(u => u.Id == ad.User.Id)
                    .FirstOrDefault();

                ad.Type = Context.AdTypes
                    .Where(t => t.ID == ad.Type.ID)
                    .FirstOrDefault();

                if (ad.Photos != null)
                {
                    Context.Photos.AddRange(ad.Photos);
                }
                Context.Adverts.Add(ad);
            }
            else
            {
                Advert dbEntry = Context.Adverts.Find(ad.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = ad.Name;
                    dbEntry.Date = DateTime.Now;
                    dbEntry.Description = ad.Description;
                    dbEntry.Price = ad.Price;
                    dbEntry.Town = ad.Town;
                    dbEntry.Status = ad.Status;
                    dbEntry.Type = Context.AdTypes.Find(ad.Type.ID);

                    if (ad.Photos != null)
                    {
                        Context.Photos.RemoveRange(dbEntry.Photos);
                        dbEntry.Photos = ad.Photos;
                        Context.Photos.AddRange(dbEntry.Photos);
                    }
                }
            }
            Context.SaveChanges();

            return ad.ID;
        }

        public Int32 GetListCount(Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus, String userID)
        {
            return Context.Adverts
                .Where(a => a.Status != excludedStatus || excludedStatus == null)
                .Where(a => a.Price >= minPrice || minPrice == null)
                .Where(a => a.Price <= maxPrice || maxPrice == null)
                .Where(a => a.Town.Replace(" ","") == town.Replace(" ", "") || town == null)
                .Where(a => a.Type.ID == adTypeID || adTypeID == null)
                .Where(a => a.User.Id == userID || userID == null)
                .Count();
        }

        public Advert GetById(Int32 id)
        {
            return Context.Adverts.FirstOrDefault(a => a.ID == id);
        }
        /*
        public ApplicationUser GetUserById(String id)
        {
            return Context.Users.Find(id);
        }*/

        public IEnumerable<Advert> GetList(Int32 page, Int32 pageSize, Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus, String userID)
        {
            return Context.Adverts.Include("Photos").Include("Type").Include("User")
                .Where(a => a.Status != excludedStatus || excludedStatus == null)
                .Where(a => a.Price >= minPrice || minPrice == null)
                .Where(a => a.Price <= maxPrice || maxPrice == null)
                .Where(a => a.Town.Trim() == town.Trim() || town == null)
                .Where(a => a.Type.ID == adTypeID || adTypeID == null)
                .Where(a => a.User.Id == userID || userID == null)
                .OrderByDescending(a => a.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<AdType> GetTypes()
        {
            return Context.AdTypes.OrderBy(n => n.Name);
        }
        /*
        public Boolean ChangeUserCredential(string id, string phoneNumber, string town, string email)
        {
            try
            {
                ApplicationUser user = Context.Users.Find(id);
                if (user != null)
                {
                    user.PhoneNumber = phoneNumber;
                    user.Town = town;
                    user.Email = email;
                }
                Context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }*/

        private ApplicationDbContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new ApplicationDbContext();
                }
                return _context;
            }
        }

        private ApplicationDbContext _context;
    }
}
