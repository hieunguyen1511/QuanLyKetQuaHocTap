using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyKetQuaHocTap
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name:"Admin",
            //    url: "{Area}/{controller}/{action}/{id}",
            //    defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
<<<<<<< HEAD
                defaults: new { controller = "DangNhap", action = "DangNhap", id = UrlParameter.Optional }
            );

=======
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
           
>>>>>>> f917e0184172b7d7be1a550e72214e58d3e1608b
        }
    }
}
