using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestMethods.Domain.Abstract
{
    // interface that holds a repository of categories
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
    }
}
