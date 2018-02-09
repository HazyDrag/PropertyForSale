using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PropertyForSale.Models;
using PropertyForSaleDomainModel.Abstract;

namespace PropertyForSale.Controllers
{
    public class AdvertController : Controller
    {
        private IRepository repository;
        public int pageSize = 2;

        public AdvertController(IRepository repo)
        {
            repository = repo;
        }

        //
        // GET: /Advert/AddAd
        public ViewResult AddAd()
        {
            return View();
        }

        //
        // POST: /Advert/AddAd
        [HttpPost]
        public ActionResult AddAd(AddAdViewModel model)
        {

            return View();
        }

        public ViewResult Ad(Int32? AdId)
        {
            try
            {
                var data = repository.Adverts
                    .First(a => a.ID == AdId);


                AdViewModel model = new AdViewModel()
                {
                    Name = data.Name,
                    Description = data.Description,
                    Price = data.Price,
                    PhoneNumber = data.User.PhoneNumber,
                    UserName = data.User.Name,
                    Photos = data.Photos.Select(p => new PhotoModel() { ID = p.ID, Path = p.Path }).ToList(),
                    Type = data.Type.Name
                };
                
                switch (data.Status.ToString())
                {
                    case "Active":
                        model.Status = AdStatusModel.Active;
                        break;
                    case "Pause":
                        model.Status = AdStatusModel.Pause;
                        break;
                    case "Stop":
                        model.Status = AdStatusModel.Stop;
                        break;
                    default:
                        return View("Error");
                }

                return View(model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ViewResult List(Int32 page = 1)
        {
            var data = repository.GetFullAdvertsData
                .Where(a => a.Status.ToString() != AdStatusModel.Stop.ToString())
                .Select(x => new AdvertModel()
                {
                    ID = x.ID,
                    Date = x.Date,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    Town = x.Town,
                    AdType = new AdTypeModel() { ID = x.Type.ID, Description = x.Type.Description, Name = x.Type.Name },
                    User = new ApplicationUserModel() { ID = x.User.Id, Name = x.User.Name },
                    Photos = x.Photos.Select(p => new PhotoModel() { ID = p.ID, Path = p.Path }).ToList(),
                    Status = x.Status.ToString() == AdStatusModel.Pause.ToString() ? AdStatusModel.Pause : AdStatusModel.Active
                });

            ListViewModel model = new ListViewModel
            {
                Adverts = data
                .OrderByDescending(advert => advert.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = data.Count()
                }
            };

            return View(model);
        }
    }
}