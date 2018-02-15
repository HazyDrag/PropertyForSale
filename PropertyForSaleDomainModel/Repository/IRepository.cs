using System;
using System.Collections.Generic;

using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Enums;

namespace PropertyForSaleDomainModel.Repository
{
    public interface IRepository
    {
        Int32 SaveAdvert(Advert ad);

        Int32 GetListCount(Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus, String userID);

        Advert GetById(Int32 id);

        IEnumerable<Advert>  GetList(Int32 page, Int32 pageSize, Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus, String userID);

        IEnumerable<AdType> GetTypes();

        //ApplicationUser GetUserById(String id);

        //Boolean ChangeUserCredential(String id, String phoneNumber, String town, String email);
    }
}
