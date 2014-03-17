using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ModestMethods.Domain.Entities
{
    // entity class for a category. Data annotations added for required fields and regex.
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a name for the category")]
        [StringLength(500, ErrorMessage = "Category name must be less than 500 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a UrlSlug for the category")]
        [StringLength(500, ErrorMessage = "UrlSlug cannot exceed 500 characters")]
        [RegularExpression(@"^[a-zA-z]+$", ErrorMessage = "The Url Slug can only contain alpha characters with no spaces")]
        public string UrlSlug { get; set; }


        public string Description { get; set; }

        [JsonIgnore]
        public IList<Post> Posts { get; set; }
    }
}
