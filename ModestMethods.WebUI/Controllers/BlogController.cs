using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModestMethods.WebUI.Models;
using System.Net.Mail;
using System.Configuration;
using System.Text;

namespace ModestMethods.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository blogRepository; // variable that holds repository of blog posts and categories
        public int PageSize = 3; // determines the amount of posts listed on each page

        // constructor that instantiates the blog repository
        public BlogController(IBlogRepository repo)
        {
            this.blogRepository = repo;
        }

        // ViewResult that passes in a category and a page # (default page # is 1)
        public ViewResult PostList(string category, int page = 1)
        {
            string currentCategory = null; // variable that holds name of current category

            // if category is not null, find category id and category name
            if (category != null)
            {
                // linq query used to match category with urlslug that was passed as category, and assign the category's name to currentCategory
                currentCategory = (from c in blogRepository.Categories
                                where c.UrlSlug == category
                                select c.Name).First().Trim();
            }

            PostsListViewModel model = new PostsListViewModel //instantiate object for ListView of posts

            {
                //linq query with lambda expressions to group posts based on category, page #, and page size. Order by latest post.
                Posts = blogRepository.Posts
                .Where(p => category == null || p.Category.UrlSlug == category)
                .Where(p => p.Published == true)
                .OrderByDescending(p => p.PostedOn)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo //instantiate object for current page info based on page #, pageSize, # of posts based on selected category
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                       blogRepository.Posts.Count() :
                       blogRepository.Posts.Where(e => e.Category.UrlSlug == category).Count()
                },
                CurrentCategory = currentCategory, // assign selected category name for use in the ListView
                CategoryName = category
            };

            return View(model); // pass the PostsListViewModel object to the View
        }

        // action that displays a single page for a selected post
        public ViewResult Post(string title)
        {
           var post = blogRepository.Post(title); //finds post based on titleslug and assigns it to var post

            // if post is null, redirect to custom error page
            if (post == null)
                Response.Redirect("~/Error/NotFound");

            //if post is not published and user is not authorized, send to custom error page
            if (post.Published == false && User.Identity.IsAuthenticated == false)
                Response.Redirect("~/Error/Unauthorized");

            return View(post); // return view for selected post
        }

        // view result for the about page
        public ViewResult About()
        {
            return View();
        }

        // view result for the contact page
        public ViewResult Contact()
        {
            return View();
        }

        // view result for when a user POSTS the contact form
        [HttpPost]
        public ViewResult Contact(Contact contact)
        {
            // if contact form is valid
            if (ModelState.IsValid)
            {
                using (var client = new SmtpClient()) // send email using...
                {
                    var adminEmail = ConfigurationManager.AppSettings["AdminEmail"]; //assigned admin email address
                    var from = new MailAddress(adminEmail, "Modest Methods Messenger"); // from assigned admin email address
                    var to = new MailAddress(adminEmail, "Modest Methods Admin"); // to assigned admin email address

                    // send message user created in contact form to admin
                    using (var message = new MailMessage(from, to))
                    {
                        message.Body = contact.Body;
                        message.IsBodyHtml = true;
                        message.BodyEncoding = Encoding.UTF8;

                        message.Subject = contact.Subject;
                        message.SubjectEncoding = Encoding.UTF8;

                        message.ReplyTo = new MailAddress(contact.Email); //set reply-address to the submitted users email.

                        client.Send(message); // send message
                    }
                }

                return View("Thanks"); // load Thanks page
            }

            return View(); // load the contact page if invalid
        }
    }
}
