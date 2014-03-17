using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ModestMethods.Domain.Entities
{
    // entity class for a blog post. DataAnnotations added for required and hidden fields.
    public class Post
    {
        [HiddenInput(DisplayValue = false)]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Please enter a title for the post")]
        [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a short description for the post")]
        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the main content for the post")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please enter the author of the post")]
        [StringLength(500, ErrorMessage = "Author name cannot exceed 100 characters")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Please enter the url you want for the post")]
        [StringLength(500, ErrorMessage = "Url slug cannot exceed 200 characters")]
        [RegularExpression(@"^[a-zA-z]+$", ErrorMessage = "The Url slug can only contain alpha characters with no spaces")]
        public string UrlSlug { get; set; }

        public bool Published { get; set; }

        [Required(ErrorMessage = "The Posted On field is required")]
        public DateTime PostedOn { get; set; }
        public DateTime? Modified { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "Please enter a category for the post")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public IList<Tag> Tags { get; set; }
    }
}
