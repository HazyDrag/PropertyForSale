using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Abstract;
using PropertyForSaleDomainModel.Enums;

namespace PropertyForSaleDomainModel.Concrete
{
    public class AdvertRepository : IRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public Int32 AddAdvert(Advert ad)
        {
            ad.User = context.Users
                .Where(u => u.Id == ad.User.Id)
                .FirstOrDefault();

            ad.Type = context.AdTypes
                .Where(t => t.ID == ad.Type.ID)
                .FirstOrDefault();

            if (ad.Photos.Count > 0)
            {
                context.Photos.AddRange(ad.Photos);
            }

            context.Adverts.Add(ad);
            context.SaveChanges();

            return ad.ID;
        }

        public Int32 GetListCount(AdFilter filter, AdStatus? excludedStatus)
        {
            return context.Adverts
                .Where(a => a.Status != excludedStatus || excludedStatus == null)
                .Where(a => a.Price >= filter.minPrice || filter.minPrice == null)
                .Where(a => a.Price <= filter.maxPrice || filter.maxPrice == null)
                .Where(a => a.Town == filter.town || filter.town == null)
                .Where(a => a.Type.ID == filter.adTypeID || filter.adTypeID == null)
                .Count();
        }

        public Advert GetById(Int32 Id)
        {
            return context.Adverts.FirstOrDefault(a => a.ID == Id);
        }

        public IEnumerable<Advert> GetList(Int32 page, Int32 pageSize, AdFilter filter, AdStatus? excludedStatus)
        {
            return context.Adverts.Include("Photos").Include("Type")
                .Where(a => a.Status != excludedStatus || excludedStatus == null)
                .Where(a => a.Price >= filter.minPrice || filter.minPrice == null)
                .Where(a => a.Price <= filter.maxPrice || filter.maxPrice == null)
                .Where(a => a.Town == filter.town || filter.town == null)
                .Where(a => a.Type.ID == filter.adTypeID || filter.adTypeID == null)
                .OrderByDescending(a => a.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<AdType> GetTypes()
        {
            return context.AdTypes.OrderBy(n => n.Name);
        }
    }
}
