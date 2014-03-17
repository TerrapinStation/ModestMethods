using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModestMethods.WebUI.Models
{
    //model for the view that lists all of the published posts
    public class PostsListViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CategoryName { get; set; }
        public string CurrentCategory { get; set; }
    }
}