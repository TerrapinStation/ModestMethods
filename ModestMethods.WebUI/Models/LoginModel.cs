using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModestMethods.WebUI.Models
{
    // model for login view using data annotations for validation.
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User name (*)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password (*)")]
        public string Password { get; set; }
    }
}