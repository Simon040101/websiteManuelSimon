using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XOVO
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) /*Gibt an wie die Url aufgebaut ist*/
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("LoginRoute", "login", new { controller = "user", action="login"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
