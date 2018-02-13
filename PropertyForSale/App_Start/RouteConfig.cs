using System.Web.Mvc;
using System.Web.Routing;

namespace PropertyForSale
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Advert", action = "List" }
            );

            routes.MapRoute(
                name: null,
                url: "Advert/Add",
                defaults: new { controller = "Advert", action = "AddAd" }
            );

            /*routes.MapRoute(
                name: null,
                url: "Advert/Search/Page{page}",
                defaults: new { controller = "Advert", action = "Search" }
            );*/

            routes.MapRoute(
                name: null,
                url: "Advert/Id/{AdId}",
                defaults: new { controller = "Advert", action = "Ad" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Advert", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
