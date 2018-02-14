using System;
using System.Collections.Generic;
using System.Linq;

using PropertyForSaleDomainModel.DbContext;
using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Enums;

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
                Context.SaveChanges();
            }
            else
            {
                //todo: finish this later
            }

            return ad.ID;
        }

        public Int32 GetListCount(Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus)
        {
            return Context.Adverts
                .Where(a => a.Status != excludedStatus || excludedStatus == null)
                .Where(a => a.Price >= minPrice || minPrice == null)
                .Where(a => a.Price <= maxPrice || maxPrice == null)
                .Where(a => a.Town == town || town == null)
                .Where(a => a.Type.ID == adTypeID || adTypeID == null)
                .Count();
        }

        public Advert GetById(Int32 Id)
        {
            return Context.Adverts.FirstOrDefault(a => a.ID == Id);
        }

        public IEnumerable<Advert> GetList(Int32 page, Int32 pageSize, Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus)
        {
            return Context.Adverts.Include("Photos").Include("Type").Include("User")
                .Where(a => a.Status != excludedStatus || excludedStatus == null)
                .Where(a => a.Price >= minPrice || minPrice == null)
                .Where(a => a.Price <= maxPrice || maxPrice == null)
                .Where(a => a.Town == town || town == null)
                .Where(a => a.Type.ID == adTypeID || adTypeID == null)
                .OrderByDescending(a => a.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<AdType> GetTypes()
        {
            return Context.AdTypes.OrderBy(n => n.Name);
        }


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
