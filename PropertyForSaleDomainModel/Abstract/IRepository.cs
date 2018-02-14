using System;
using System.Collections.Generic;

using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Enums;

namespace PropertyForSaleDomainModel.Abstract
{
    public interface IRepository
    {
        Int32 SaveAdvert(Advert Ad);

        Int32 GetListCount(Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus);

        Advert GetById(Int32 Id);

        IEnumerable<Advert>  GetList(Int32 page, Int32 pageSize, Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus);

        IEnumerable<AdType> GetTypes();
    }
}
