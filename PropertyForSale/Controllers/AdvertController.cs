using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using PropertyForSale.Enums;
using PropertyForSale.Models;
using PropertyForSaleDomainModel.Entities;
using PropertyForSaleDomainModel.Repository;

namespace PropertyForSale.Controllers
{
    public class AdvertController : Controller
    {
        private IRepository _repository;
        private Int32 _pageSize = 2;
        private String _defaultPhotoPath = "/Images/Ad/";
        private String _originalPhotoPath = "Original/";
        private String _standartPhotoPath = "Standart/";
        private String _smallPhotoPath = "Small/";
        private String _defaultPhotoName = "defaultAdImage.jpg";
        
        public AdvertController(IRepository repo)
        {
            _repository = repo;
        }

        private List<AdTypeModel> GetTypesOfProperty()
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
                Adverts = _repository.GetList(page, _pageSize, minPrice, maxPrice, adTypeID, town, excludedStatus, userID)
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
                    Photos = GetPhotosOfAdvert(x, PhotoSize.Standart),
                    User = new ApplicationUserModel
                    {
                        ID = x.User.Id
                    }
                }).ToList(),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = _pageSize,
                    TotalItems = _repository.GetListCount(minPrice, maxPrice, adTypeID, town, excludedStatus, userID)
                }
            };
        }

        private List<PhotoModel> GetPhotosOfAdvert(Advert x, PhotoSize photoSize)
        {
            String photoSizePath = 
                photoSize == PhotoSize.Standart ? _standartPhotoPath
                : photoSize == PhotoSize.Small ? _smallPhotoPath
                : _originalPhotoPath;

            List<PhotoModel> photos;

            if (x.Photos.Count > 0)
            {
                photos = x.Photos.Select(p => new PhotoModel
                {
                    ID = p.ID,
                    Path = p.Path,
                }).ToList();
            }
            else
            {
                photos = new List<PhotoModel>
                {
                    new PhotoModel
                    {
                        Path = _defaultPhotoName,
                    }
                };
            }

            foreach (PhotoModel p in photos)
            {
                String fullPhotoSizePath = Server.MapPath(_defaultPhotoPath + photoSizePath + p.Path);
                String fullOriginalPhotoPath = Server.MapPath(_defaultPhotoPath + _originalPhotoPath + p.Path);

                if (!System.IO.File.Exists(fullPhotoSizePath))
                {
                    Image imgPhoto = Image.FromFile(fullOriginalPhotoPath);
                    
                    ReSize(imgPhoto, photoSize).Save(fullPhotoSizePath);
                }
                p.Path = _defaultPhotoPath + photoSizePath + p.Path;
            }

            return photos;
        }

        private Photo SavePhoto(HttpPostedFileBase photo)
        {
            Photo savedPhoto = null;

            // Save image to folder and get path
            if (photo != null)
            {
                var imageName = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                var extension = Path.GetExtension(photo.FileName).ToLower();
                using (var img = Image.FromStream(photo.InputStream))
                {
                    String filename = String.Format("{0}{1}", imageName, extension);

                    using (Image newImg = new Bitmap(img))
                    {
                        try
                        {
                            newImg.Save(Server.MapPath(_defaultPhotoPath + _originalPhotoPath + filename), img.RawFormat);
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

        private Image ReSize(Image imgPhoto, PhotoSize photoSize)
        {
            Int32 width;
            Int32 height;
            Int32 sourceWidth = imgPhoto.Width;
            Int32 sourceHeight = imgPhoto.Height;
            Int32 sourceX = 0;
            Int32 sourceY = 0;
            Int32 destX = 0;
            Int32 destY = 0;

            Single nPercent = 0;
            Single nPercentW = 0;
            Single nPercentH = 0;

            if (photoSize == PhotoSize.Standart)
            {
                width = 500;
                height = 333;
            }
            else
            {
                width = 280;
                height = 186;
            }

            nPercentW = ((Single)width / (Single)sourceWidth);
            nPercentH = ((Single)height / (Single)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((height -
                              (sourceHeight * nPercent)) / 2);
            }

            Int32 destWidth = (Int32)(sourceWidth * nPercent);
            Int32 destHeight = (Int32)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
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
                OldPhoto = GetPhotosOfAdvert(data, PhotoSize.Small)[0]
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
                model.OldPhoto = GetPhotosOfAdvert(data, PhotoSize.Small)[0];

                return View(model);
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

            Photo newPhoto = SavePhoto(model.NewPhoto);
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

            Photo newPhoto = SavePhoto(model.NewPhoto);
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
                Photos = GetPhotosOfAdvert(data, PhotoSize.Standart),
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
                    ItemsPerPage = _pageSize,
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