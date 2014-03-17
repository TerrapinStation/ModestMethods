using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModestMethods.WebUI.HtmlHelpers
{
    public static class Extensions
    {
        public static string Href(this Post post, UrlHelper helper)
        {
            return helper.RouteUrl(new { controller = "Blog", action = "Post", title = post.UrlSlug});
        }
    }
}