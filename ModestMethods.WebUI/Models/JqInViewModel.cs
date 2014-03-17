using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModestMethods.WebUI.Models
{
    // model used for defining data that will be used for interacting with the jqgrid
    public class JqInViewModel
    {
        public int rows { get; set; } // no. of row based on records being loaded
        public int page { get; set; } // no. of pages
        public string sidx { get; set; } // used for sort column name
        public string sord { get; set; } // used for sorting by asc or desc
    }
}