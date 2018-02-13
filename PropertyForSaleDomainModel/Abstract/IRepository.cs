using System;
using System.Collections.Generic;

using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Enums;

namespace PropertyForSaleDomainModel.Abstract
{
    public interface IRepository
    {
        Int32 AddAdvert(Advert Ad);

        Int32 GetListCount(AdFilter filter, AdStatus? excludedStatus);

        Advert GetById(Int32 Id);

        IEnumerable<Advert>  GetList(Int32 page, Int32 pageSize, AdFilter filter, AdStatus? excludedStatus);

        IEnumerable<AdType> GetTypes();
    }
}
