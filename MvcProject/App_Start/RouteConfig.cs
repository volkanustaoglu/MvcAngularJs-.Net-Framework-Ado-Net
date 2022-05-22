using MvcProject.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "Gallery",
               url: "Gallery/{id}",
               defaults: new { controller = "Gallery", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Tag",
               url: "Tag/{id}",
               defaults: new { controller = "Tag", action = "Index", id = UrlParameter.Optional }
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
       
    }
}
