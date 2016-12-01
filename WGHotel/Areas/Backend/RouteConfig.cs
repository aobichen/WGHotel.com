using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WGHotel.Areas.Backend
{
    internal static class RouteConfig
    {
        internal static void RegisterRoutes(AreaRegistrationContext context)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            context.MapRoute(
                "Room",
                 "Backend/Room/{id}",
                new {controller = "Room", action = "Index", id = UrlParameter.Optional });

            context.MapRoute(
               "Backend_default",
               "Backend/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );

        }
    }
}