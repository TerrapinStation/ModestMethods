using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestMethods.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private BlogDbContext context = new BlogDbContext();

        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }
    }
}
