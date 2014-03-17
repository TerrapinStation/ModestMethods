using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModestMethods.WebUI.Controllers
{
    // custom error controller to display views based on error code in the web.config under system.webServer
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult NotFound()
        {
            return View();
        }

        public ViewResult Forbidden()
        {
            return View();
        }

        public ViewResult Unauthorized()
        {
            return View();
        }
    }
}
