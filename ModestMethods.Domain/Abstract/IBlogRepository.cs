using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestMethods.Domain.Abstract
{
    // interface that holds a repository of posts and categories, and allows the WebUI to bind and use the methods listed here and defined in EFBlogRepository.
    public interface IBlogRepository
    {
        IQueryable<Post> Posts { get; } // provides a searchable list of posts

        Post Post(string titleSlug); // provides a post based on the title

        IList<Post> AdminPost(int pageNo, int pageSize, string sortColumn, bool sortByAscending);  // provides a list of Posts for sorting the jqGrid

        int TotalPosts(bool checkIsPublished = true); // provides a number of total posts that are published

        int AddPost(Post post); // provides a function that adds a post

        void EditPost(Post post); // provides a function that edits a post

        void DeletePost(int postId); // provides a function that deletes a post

        IQueryable<Category> Categories { get; } // provides a searchable list of categories

        int AddCategory(Category category); // provides a function that adds a category

        void EditCategory(Category category); //provides a function that edits a category

        void DeleteCategory(int categoryId); // provides a function that deletes a category

        IQueryable<Tag> Tags { get; }

        IQueryable<PostTagMap> PostTagMap { get; }

        int AddTag(Tag tag); // provides a function that adds a tag

        void EditTag(Tag tag); // provides a function to edit a tag

        void DeleteTag(int tagId); // provides a function that deletes a post
    }
}
