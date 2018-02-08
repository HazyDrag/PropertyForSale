using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Abstract;

namespace PropertyForSaleDomainModel.Concrete
{
    public class PropertyRepository : IRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<Advert> Adverts
        {
            get { return context.Adverts; }
        }
    }
}
