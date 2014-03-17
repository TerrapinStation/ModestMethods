using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ModestMethods.WebUI.HtmlHelpers
{
    public static class ActionLinkExtensions
    {
        //html helper method that takes a Post object and turns it into a link with a css id using the posts title
        public static MvcHtmlString PostLink(this HtmlHelper helper, Post post)
        {
            return helper.ActionLink(post.Title, "Post", "Blog", new { title = post.UrlSlug }, new { title = post.Title, id = "post-link" });
        }
    }
}