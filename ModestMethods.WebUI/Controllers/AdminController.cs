using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Entities;
using ModestMethods.WebUI.Models;
using System.Web.Security;
using Newtonsoft.Json;
using ModestMethods.WebUI.Helpers;
using System.Text;

namespace ModestMethods.WebUI.Controllers
{
    // authorized for so only credentialed admins can access the actions within the AdminController.
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IBlogRepository blogRepository; //variable that holds repository of blog posts and categories and allows access to methods for CRUD

        public AdminController(IBlogRepository repo)
        {
            this.blogRepository = repo; //instantiates the blogRepository
        }

        // action result for the login view.
        [AllowAnonymous] // allows non-admin to access the login screen.
        public ActionResult Login(string returnUrl)
        {
            // if the user is authenticated, send them to the page requested. Or if there was no page requested, send them to the "Manage" page
            if (User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Manage");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(); //else return the Login page
        }

        //action result for user sending a POST of login credentials
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken] // POST action, allows non-admin users to POST, and secures credentials using AntiForgeryToken
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (FormsAuthentication.Authenticate(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Manage");
                }

                ModelState.AddModelError("", "The user name or password provided is incorrect"); // display error if credentials are incorrect
            }

            return View();
        }

        // action that logouts a user and sends them back to the login page
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Admin");
        }

        // action that displays the manage page
        public ActionResult Manage()
        {
            return View();
        }

        // content result used to get a list of posts based on the parameters sent from the jqGrid and the total # of posts
        public ContentResult Posts(JqInViewModel jqParams)
        {
            var posts = blogRepository.AdminPost(jqParams.page - 1, jqParams.rows, jqParams.sidx, jqParams.sord == "asc");

            var totalPosts = blogRepository.TotalPosts(false);

            // serializes a json dataset to be used in a jqgrid
            return Content(JsonConvert.SerializeObject(new
            {
                page = jqParams.page,
                records = totalPosts,
                rows = posts,
                total = Math.Ceiling(Convert.ToDouble(totalPosts) / jqParams.rows)
            }, new CustomDateTimeConverter()), "application/json"); //custom time converter used to convert newtonsoft.json data to
        }

        // content result that adds a post to the database. By default MVC, prevents sending data that contains potentially
        //malicious content. Since we are passing info with html tags, we have to set ValidateInput to false.
        [HttpPost, ValidateInput(false)]
        public ContentResult AddPost(Post post)
        {
            string json; //variable for passing successful or unsuccessful json data

            ModelState.Clear(); //remove items from the model state dictionary

            // if the current post is valid
            if (TryValidateModel(post))
            {
                post.PostedOn = DateTime.Now; // assign the new post a posted on date based on current time
                var id = blogRepository.AddPost(post); // get id from newly added post and attempt to save data

                // if id equals the new id, post was successful
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Post added succesfully."
                });
            }
                //else if id is 0, post was not successful
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the post."
                });
            }

            return Content(json, "application/json"); //return the json data to show user if post was successful or not
        }

        // content result that edits a post and saves the changes to the database. By default, MVC prevents sending data that contains potentially
        //malicious content. Since we are passing info with html tags, we have to set ValidateInput to false.
        [HttpPost, ValidateInput(false)]
        public ContentResult EditPost(Post post)
        {
            string json; //variable for passing successful or unsuccessful json data

            ModelState.Clear(); //remove items from the model state dictionary

            // if the current post is valid
            if (TryValidateModel(post))
            {
                blogRepository.EditPost(post); // get id from newly edited post and attempt to save data

                // if id = edited post id, change was successful
                json = JsonConvert.SerializeObject(new
                {
                    id = post.PostId,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
                //else if id = 0, changes failed.
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json"); //return the json data to show user if editing the post was successful or not
        }

        // content result that deletes a post and saves the changes to the database.
        [HttpPost]
        public ContentResult DeletePost(int id)
        {
            blogRepository.DeletePost(id); //delete post that was passed to the action.

            // if id = 0, deletion was successful
            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Post deleted successfully."
            });

            return Content(json, "application/json"); // send results to the user for display
        }

        // content result used to get a list of categories
        public ContentResult Categories()
        {
            var categories = blogRepository.Categories; //instantiate list of categories

            // serializes and returns a json dataset to be used in a jqgrid
            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = categories.Count(),
                rows = categories,
                total = 1
            }), "application/json");
        }

        // content result that adds a category and saves the changes to the database.
        [HttpPost]
        public ContentResult AddCategory(Category category)
        {
            string json; //variable for passing successful or unsuccessful json data

            ModelState.Clear(); //remove items from the model state dictionary

            // if the current category is valid
            if (TryValidateModel(category))
            {
                var id = blogRepository.AddCategory(category); // get id from newly add category and attempt to save data

                // if id equals the new id, category addition was successful
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Category added succesfully."
                });
            }
                // else if id = 0; adding the category failed
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the category."
                });
            }

            return Content(json, "application/json"); // send results to the user for display
        }

        // content result that edits a category and saves the changes to the database.
        [HttpPost]
        public ContentResult EditCategory(Category category)
        {
            string json; //variable for passing successful or unsuccessful json data

            ModelState.Clear(); //remove items from the model state dictionary

            // if the current category is valid
            if (TryValidateModel(category))
            {
                blogRepository.EditCategory(category); // get id from newly edited category and attempt to save data

                // if id equals the edited category id, category addition was successful
                json = JsonConvert.SerializeObject(new
                {
                    id = category.CategoryId,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
                //else if the id = 0, the edit failed
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json"); // send results to the user for display
        }

        // content result that deletes a category and saves the changes to the database.
        [HttpPost]
        public ContentResult DeleteCategory(int id)
        {
            blogRepository.DeleteCategory(id); // delete category based on passed Id (deletes associated posts too)

            // if id = 0, deletion was successful
            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Category deleted successfully."
            });

            return Content(json, "application/json"); // send results to the user for display
        }

        // list of categories to create category dropdown for when adding/editing Posts
        public ContentResult GetCategoriesHtml()
        {
            var categories = blogRepository.Categories.OrderBy(s => s.Name); // get categories ordered by name

            var sb = new StringBuilder(); //instantiate a stringbuilder
            sb.AppendLine(@"<select>"); //add a select attribute to the string to create dropdown

            //for each categoryh append to the string the Id and Name of categeory, which adds to the dropdown
            foreach (var category in categories)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>",
                    category.CategoryId, category.Name));
            }

            sb.AppendLine("<select>"); //close select attribute
            return Content(sb.ToString(), "text/html"); // return dropdown list as html
        }

        // content result used to get a list of tags
        public ContentResult Tags()
        {
            var tags = blogRepository.Tags;

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = tags.Count(),
                rows = tags,
                total = 1
            }), "application/json");
        }

        // content result that adds a tag and saves the changes to the database.
        [HttpPost]
        public ContentResult AddTag(Tag tag)
        {
            string json; //variable for passing successful or unsuccessful json data

            ModelState.Clear(); //remove items from the model state dictionary

            // if the current category is valid
            if (TryValidateModel(tag))
            {
                var id = blogRepository.AddTag(tag); // get id from newly add tag and attempt to save data

                // if id equals the new id, category addition was successful
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Category added succesfully."
                });
            }
            // else if id = 0; adding the tag failed
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the category."
                });
            }

            return Content(json, "application/json"); // send results to the user for display
        }

        // content result that edits a tag and saves the changes to the database.
        [HttpPost]
        public ContentResult EditTag(Tag tag)
        {
            string json; //variable for passing successful or unsuccessful json data

            ModelState.Clear(); //remove items from the model state dictionary

            // if the current category is valid
            if (TryValidateModel(tag))
            {
                blogRepository.EditTag(tag); // get id from newly edited tag and attempt to save data

                // if id equals the edited category id, category addition was successful
                json = JsonConvert.SerializeObject(new
                {
                    id = tag.TagId,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            //else if the id = 0, the edit failed
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json"); // send results to the user for display
        }

        // content result that deletes a tag and saves the changes to the database.
        [HttpPost]
        public ContentResult DeleteTag(int id)
        {
            blogRepository.DeleteTag(id); // delete tag based on passed Id (deletes connection to associated posts too)

            // if id = 0, deletion was successful
            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Category deleted successfully."
            });

            return Content(json, "application/json"); // send results to the user for display
        }

        // list of tags to create tag selector for when adding/editing Posts
        public ContentResult GetTagsHtml()
        {
            var tags = blogRepository.Tags.OrderBy(s => s.Name);

            var sb = new StringBuilder();
            sb.AppendLine(@"<select multiple=""multiple"">");

            foreach (var tag in tags)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>", tag.TagId, tag.Name));
            }

            sb.AppendLine("<select>");
            return Content(sb.ToString(), "text/html");
        }
    }
}
