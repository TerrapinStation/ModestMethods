using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ModestMethods.WebUI
{
    // custom routes for the differnet controllers and actions so that clean urls are used
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Admin", action = "Login" });

            routes.MapRoute(
              "Logout",
              "Logout",
              new { controller = "Admin", action = "Logout" });

            routes.MapRoute(
                "About",
                "About",
                new { controller = "Blog", action = "About" });

            routes.MapRoute(
                "Contact",
                "Contact",
                new { controller = "Blog", action = "Contact" });

            routes.MapRoute("Post",
                "Blog/{title}",
                new { controller = "Blog", action = "Post" });

            routes.MapRoute(null,
                "",
                new { controller = "Blog", action = "PostList", category = (string)null, page = 1 });

            routes.MapRoute(null,
                "Page{page}",
                new {controller = "Blog", action = "PostList", category = (string)null },
                new { page = @"\d+" });

            routes.MapRoute(null,
                "Category/{category}",
                new { controller = "Blog", action = "PostList", page = 1 });

            routes.MapRoute(null,
                "Category/{category}/Page{page}",
                new { controller = "Blog", action = "PostList" },
                new { page = @"\d+" });

            routes.MapRoute(null, "{controller}/{action}");

        }
    }
}