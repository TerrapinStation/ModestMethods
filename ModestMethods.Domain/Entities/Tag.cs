using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestMethods.Domain.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public IList<Post> Posts { get; set; }
    }
}
