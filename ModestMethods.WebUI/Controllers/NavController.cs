using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModestMethods.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBlogRepository repository; // variable that holds repository of categories

        // constructor that instantiates the category repository
        public NavController(IBlogRepository repo)
        {
            repository = repo;
        }

        // partialview that is used to display the different categories in the nav menu.
        public PartialViewResult Menu(string category = null)
        {
            string categoryName = null;

            // if category is not null, use a linq query to get the selected category
            if (category != null)
            {
                categoryName = (from c in repository.Categories
                                where c.UrlSlug == category
                                select c.Name).First().Trim();
            }
            
            ViewBag.SelectedCategory = categoryName; // pass category name to viewbag for title change
            IEnumerable<Category> categories = repository.Categories; // pass all categories into a IEnumerable list
            return PartialView(categories); // pass all categories to the partialview
        }

    }
}
