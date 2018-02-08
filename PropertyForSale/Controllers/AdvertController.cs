using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PropertyForSaleDomainModel.Abstract;

namespace PropertyForSale.Controllers
{
    public class AdvertController : Controller
    {
        private IRepository repository;
        
        public AdvertController(IRepository repo)
        {
            repository = repo;
        }

        public ActionResult List()
        {
            return View(repository.Adverts);
        }
    }
}