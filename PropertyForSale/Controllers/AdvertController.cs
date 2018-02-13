using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using PropertyForSale.Models;
using PropertyForSaleDomainModel.Abstract;
using Microsoft.AspNet.Identity;
using System.Drawing;
using PropertyForSaleDomainModel.Enums;
using PropertyForSaleDomainModel.Entities;

namespace PropertyForSale.Controllers
{
    public class AdvertController : Controller
    {
        private IRepository repository;
        public int pageSize = 2;

        private void SaveToFolder(Image img, string pathToSave)
        {
            // Get new resolution
            using (System.Drawing.Image newImg = new Bitmap(img))
            {
                newImg.Save(Server.MapPath("~/Images/Ad/" + pathToSave), img.RawFormat);
            }
        }

        public AdvertController(IRepository repo)
        {
            repository = repo;
        }

        //
        // GET: /Advert/AddAd
        public ActionResult AddAd()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            //get list of property types
            var data = repository.GetTypes()
                .Select(x => new AdTypeModel
                {
                    ID = x.ID,
                    Name = x.Name,
                });

            AddAdViewModel model = new AddAdViewModel
            {
                Types = data.ToList()
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
                //NEED TO FIX
                model.Types=repository.GetTypes()
                .Select(x => new AdTypeModel
                {
                    ID = x.ID,
                    Name = x.Name,
                }).ToList();
                
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

            // Save image to folder and get path
            if (model.Photo != null)
            {
                var imageName = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                var extension = System.IO.Path.GetExtension(model.Photo.FileName).ToLower();
                using (var img = System.Drawing.Image.FromStream(model.Photo.InputStream))
                {
                    String filename = String.Format("{0}{1}", imageName, extension);
                    advert.Photos = new List<Photo>();
                    advert.Photos.Add(new Photo
                    {
                        Path = filename
                    });
                    SaveToFolder(img, filename);
                }
            }
    
            Int32 modelID = repository.AddAdvert(advert);

            return RedirectToAction("Ad", new { adId = modelID });
        }

        public ViewResult Ad(Int32 adId)
        {
            try
            {
                var data = repository.GetById(adId);
                  
                AdViewModel model = new AdViewModel()
                {
                    Name = data.Name,
                    Description = data.Description,
                    Price = data.Price,
                    PhoneNumber = data.User.PhoneNumber,
                    UserName = data.User.Name,
                    Photos = data.Photos.Select(p => new PhotoModel()
                    {
                        ID = p.ID,
                        Path = p.Path
                    }).ToList(),
                    Type = data.Type.Name,
                    Status = data.Status
                };

                return View(model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ViewResult List(Int32 page = 1)
        {
            var data = repository.GetList(page, pageSize, new AdFilter(), AdStatus.Stop)
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
                    Photos = x.Photos.Select(p => new PhotoModel()
                    {
                        ID = p.ID,
                        Path = p.Path,
                    }).ToList()
                });

            ListViewModel model = new ListViewModel
            {
                Adverts = data,

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.GetListCount(new AdFilter(), AdStatus.Stop)
                }
            };

            return View(model);
        }
    }
}