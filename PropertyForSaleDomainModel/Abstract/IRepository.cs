using System;
using System.Collections.Generic;

using PropertyForSaleDomainModel.Entities;

namespace PropertyForSaleDomainModel.Abstract
{
    public interface IRepository
    {
        IEnumerable<Advert> Adverts { get; }

        IEnumerable<Advert> GetFullAdvertsData { get; }
    }
}
