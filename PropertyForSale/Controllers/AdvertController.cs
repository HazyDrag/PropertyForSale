using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using PropertyForSale.Models;
using PropertyForSaleDomainModel.Enums;
using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Repository;

namespace PropertyForSale.Controllers
{
    public class AdvertController : Controller
    {
        private IRepository _repository;
        public Int32 pageSize = 2;
        public String defaultPhotoPath = "/Images/Ad/";
        public String defaultPhotoName = "defaultAdImage.jpg";
        
        public AdvertController(IRepository repo)
        {
            _repository = repo;
        }

        public List<AdTypeModel> GetTypesOfProperty()
        {
            return _repository.GetTypes()
                .Select(x => new AdTypeModel
                {
                    ID = x.ID,
                    Name = x.Name,
                }).ToList();
        }

        public List<PhotoModel> GetPhotosOfAdvert(Advert x)
        {
            return x.Photos.Count > 0 
                ? x.Photos.Select(p => new PhotoModel
                {
                    ID = p.ID,
                    Path = defaultPhotoPath + p.Path,
                }).ToList()
                : new List<PhotoModel>
                {
                    new PhotoModel
                    {
                        Path = defaultPhotoPath + defaultPhotoName,
                    }
                };
        }

        public Photo SavedPhoto(HttpPostedFileBase photo)
        {
            Photo savedPhoto = null;

            // Save image to folder and get path
            if (photo != null)
            {
                var imageName = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                var extension = System.IO.Path.GetExtension(photo.FileName).ToLower();
                using (var img = System.Drawing.Image.FromStream(photo.InputStream))
                {
                    String filename = String.Format("{0}{1}", imageName, extension);

                    using (System.Drawing.Image newImg = new Bitmap(img))
                    {
                        try
                        {
                            newImg.Save(Server.MapPath(defaultPhotoPath + filename), img.RawFormat);
                            savedPhoto = new Photo
                            {
                                Path = filename
                            };
                        }
                        catch
                        {
                            savedPhoto = null;
                        }
                    }
                }
            }
            return savedPhoto;
        }


        //
        // GET: /Advert/Edit
        public ViewResult Edit(Int32 AdId)
        {
            if (!User.Identity.IsAuthenticated || User.Identity.GetUserId() != _repository.GetById(AdId).User.Id)
            {
                return View("Error");
            }

            var data = _repository.GetById(AdId);
            EditAdViewModel model = new EditAdViewModel
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                Status = data.Status,
                TypeID = data.Type.ID,
                Town = data.Town,
                Types = GetTypesOfProperty(),
                OldPhoto = GetPhotosOfAdvert(data)[0]
            };

            return View(model);
        }

        //
        // GET: /Advert/AddAd
        public ActionResult AddAd()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            AddAdViewModel model = new AddAdViewModel
            {
                Types = GetTypesOfProperty()
            };

            return View(model);
        }

        //
        // POST: /Advert/AddAd
        [HttpPost]
        public ActionResult AddAd(AddAdViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                model.Types = GetTypesOfProperty();
                
                return View(model);
            }

            Advert advert = new Advert
            {
                Name = model.Name,
                Town = model.Town,
                Description = model.Description,
                Price = model.Price,
                Date = DateTime.Now,
                Type = new AdType
                {
                    ID = model.TypeID
                },
                Status = model.Status,
                User = new ApplicationUser
                {
                    Id = User.Identity.GetUserId()
                }
            };

            Photo newPhoto = SavedPhoto(model.NewPhoto);
            if (newPhoto != null)
            {
                advert.Photos = new List<Photo>
                    {
                        newPhoto
                    };
            }

            Int32 modelID = _repository.SaveAdvert(advert);

            return RedirectToAction("Ad", new { adId = modelID });
        }

        //
        // GET: /Advert/Id/{Id}
        public ViewResult Ad(Int32 adId)
        {
            try
            {
                var data = _repository.GetById(adId);

                AdvertModel model1 = new AdvertModel
                {
                    ID = adId,
                    Name = data.Name,
                    Description = data.Description,
                    Price = data.Price,
                    UserID = data.User.Id,
                    Photos = GetPhotosOfAdvert(data),
                    AdType = new AdTypeModel
                    {
                        Name = data.Type.Name
                    },
                    Status = data.Status
                };
                  
                AdViewModel model = new AdViewModel
                {
                    ID = adId,
                    Name = data.Name,
                    Description = data.Description,
                    Price = data.Price,
                    UserName = "",
                    PhoneNumber = "",
                    Photos = GetPhotosOfAdvert(data),
                    UserID = data.User.Id,
                    Type = data.Type.Name,
                    Status = data.Status
                };

                if(model.Status == AdStatus.Active)
                {
                    model.UserName = data.User.Name;
                    model.PhoneNumber = data.User.PhoneNumber;
                }

                return View(model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        //
        //GET: /
        public ViewResult List(Int32 page = 1)
        {
            var data = _repository.GetList(page, pageSize, null, null, null, null, AdStatus.Stop)
                .Select(x => new AdvertModel()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Price = x.Price,
                    AdType = new AdTypeModel()
                    {
                        ID = x.Type.ID,
                        Description = x.Type.Description,
                        Name = x.Type.Name
                    },
                    Photos = GetPhotosOfAdvert(x),
                    UserID = x.User.Id
                });

            ListViewModel model = new ListViewModel
            {
                Adverts = data,

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = _repository.GetListCount(null, null, null, null, AdStatus.Stop)
                }
            };

            return View(model);
        }
        
        //
        //GET: Advert/Search/
        public ViewResult Search(SearchViewModel modelWithFilter = null, Int32 page = 1)
        {
            if (!ModelState.IsValid)
            {
                modelWithFilter.Types = GetTypesOfProperty();
                modelWithFilter.Adverts = new List<AdvertModel>();
                modelWithFilter.PagingInfo = new PagingInfo()
                {
                    TotalItems = 0,
                    ItemsPerPage = pageSize,
                    CurrentPage = 0
                };

                return View(modelWithFilter);
            }

            if (modelWithFilter == null)
            {
                modelWithFilter = new SearchViewModel();
            }

            SearchViewModel model = new SearchViewModel()
            {
                MinPrice = modelWithFilter.MinPrice,
                MaxPrice = modelWithFilter.MaxPrice,
                AdTypeID = modelWithFilter.AdTypeID,
                Town = modelWithFilter.Town,
                Types = GetTypesOfProperty(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = _repository.GetListCount(modelWithFilter.MinPrice, modelWithFilter.MaxPrice, modelWithFilter.AdTypeID, modelWithFilter.Town, AdStatus.Stop)
                },
                Adverts = _repository.GetList(page, pageSize, modelWithFilter.MinPrice, modelWithFilter.MaxPrice, modelWithFilter.AdTypeID, modelWithFilter.Town, AdStatus.Stop)
                .Select(x => new AdvertModel()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Price = x.Price,
                    AdType = new AdTypeModel()
                    {
                        ID = x.Type.ID,
                        Description = x.Type.Description,
                        Name = x.Type.Name
                    },
                    UserID = x.User.Id,
                    Photos = GetPhotosOfAdvert(x)
                })
            };

            return View(model);
        }
    }
}