using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLNSService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "nhapslcd",
                url: "nhap-san-luong-cong-doan",
                defaults: new { controller = "InsertProductivity", action = "InsertPhaseQuantity", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
              name: "sp",
              url: "sanpham",
              defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}