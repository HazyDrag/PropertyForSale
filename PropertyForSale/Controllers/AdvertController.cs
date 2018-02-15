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

        private ListViewModel GetFilteredListViewModel(Int32 page, Int32? minPrice, Int32? maxPrice, Int32? adTypeID, String town, AdStatus? excludedStatus, String userID)
        {
            return new ListViewModel
            {
                Adverts = _repository.GetList(page, pageSize, minPrice, maxPrice, adTypeID, town, excludedStatus, userID)
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
                    User = new ApplicationUserModel
                    {
                        ID = x.User.Id
                    }
                }).ToList(),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = _repository.GetListCount(minPrice, maxPrice, adTypeID, town, excludedStatus, userID)
                }
            };
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
        [Authorize]
        public ViewResult Edit(Int32 AdId)
        {
            var data = _repository.GetById(AdId);

            if (data == null)
            {
                return View("Error404");
            }

            if (User.Identity.GetUserId() != data.User.Id)
            {
                return View("AccessError");
            }

            EditAdViewModel model = new EditAdViewModel
            {
                Id = AdId,
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
        // POST: /Advert/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditAdViewModel model, Int32 id)
        {
            var data = _repository.GetById(id);
            if (data == null)
            {
                return View("Error404");
            }
            if (User.Identity.GetUserId() != data.User.Id)
            {
                return View("AccessError");
            }
            if (!ModelState.IsValid)
            {
                model.Types = GetTypesOfProperty();
                model.Id = id;
                model.OldPhoto = GetPhotosOfAdvert(data)[0];

                View(model);
            }

            Advert advert = new Advert
            {
                ID = id,
                Name = model.Name,
                Date = DateTime.Now,
                Description = model.Description,
                Price = model.Price,
                Town = model.Town,
                Status = model.Status,
                Type = new AdType
                {
                    ID = model.TypeID
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
        // GET: /Advert/AddAd
        [Authorize]
        public ViewResult AddAd()
        {
            AddAdViewModel model = new AddAdViewModel
            {
                Types = GetTypesOfProperty()
            };

            return View(model);
        }

        //
        // POST: /Advert/AddAd
        [HttpPost]
        [Authorize]
        public ActionResult AddAd(AddAdViewModel model)
        {
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
            var data = _repository.GetById(adId);

            if (data == null)
            {
                return View("Error404");
            }
            if (data.Status == AdStatus.Stop)
            {
                if (!Request.IsAuthenticated || User.Identity.GetUserId() != data.User.Id)
                {
                    return View("AccessError");
                }
            }

            AdvertModel model = new AdvertModel
            {
                ID = adId,
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                User = new ApplicationUserModel
                {
                    ID = data.User.Id,
                },
                Photos = GetPhotosOfAdvert(data),
                AdType = new AdTypeModel
                {
                    Name = data.Type.Name
                },
                Status = data.Status
            };

            if (model.Status == AdStatus.Active)
            {
                model.User.Name = data.User.Name;
                model.User.PhoneNumber = data.User.PhoneNumber;
            }

            return View(model);
        }

        //
        //GET: /
        public ViewResult List(Int32 page = 1)
        {
            if (page < 1)
            {
                return View("Error404");
            }
            ListViewModel model = GetFilteredListViewModel(page, null, null, null, null, AdStatus.Stop, null);

            ViewBag.Title = "List of advertisements";
            ViewBag.CurrentAction = "List";

            return View(model);
        }

        [Authorize]
        public ViewResult MyList(Int32 page = 1)
        {
            if (page < 1)
            {
                return View("Error404");
            }
            var userID = User.Identity.GetUserId();
            
            ListViewModel model = GetFilteredListViewModel(page, null, null, null, null, null, userID);

            ViewBag.Title = "My advertisements";
            ViewBag.CurrentAction = "MyList";

            return View("List", model);
        }
        
        //
        //GET: Advert/Search/
        public ViewResult Search(SearchViewModel modelWithFilter = null, Int32 page = 1)
        {
            if (page < 1)
            {
                return View("Error404");
            }
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

            ListViewModel listModel = GetFilteredListViewModel(page, modelWithFilter.MinPrice, modelWithFilter.MaxPrice, modelWithFilter.AdTypeID, modelWithFilter.Town, AdStatus.Stop, null);

            SearchViewModel model = new SearchViewModel()
            {
                MinPrice = modelWithFilter.MinPrice,
                MaxPrice = modelWithFilter.MaxPrice,
                AdTypeID = modelWithFilter.AdTypeID,
                Town = modelWithFilter.Town,
                Types = GetTypesOfProperty(),
                PagingInfo = listModel.PagingInfo,
                Adverts = listModel.Adverts
            };

            return View(model);
        }
    }
}