using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ModestMethods.Domain.Concrete
{
    //concrete class derived from IBlogRepository interface
    public class EFBlogRepository : IBlogRepository
    {
        private BlogDbContext context = new BlogDbContext(); // connect to database

        //  provides a searchable list of Posts
        public IQueryable<Post> Posts
        {
            get { return context.Posts; }
        }

        // returns a post based on the urlSlug (the urlSlug should be based on Post.title)
        public Post Post(string titleSlug)
        {
            var query = context.Posts
                .Where(p => p.UrlSlug == titleSlug);

            return query.Single();
        }

        // able to returns various asc and desc sorted lists of posts based on the selection from the jqGrid
        public IList<Post> AdminPost(int pageNo, int pageSize, string sortColumn,
                                    bool sortByAscending)
        {
            IQueryable<Post> query = context.Posts;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                        query = context.Posts
                                        .OrderBy(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    else
                        query = context.Posts
                                        .OrderByDescending(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    break;

                case "Published":
                    if (sortByAscending)
                        query = context.Posts
                                        .OrderBy(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    else
                        query = context.Posts
                                        .OrderByDescending(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    break;

                case "PostedOn":
                    if (sortByAscending)
                        query = context.Posts
                                        .OrderBy(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    else
                        query = context.Posts
                                        .OrderByDescending(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    break;

                case "Modified":
                    if (sortByAscending)
                        query = context.Posts
                                        .OrderBy(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    else
                        query = context.Posts
                                        .OrderByDescending(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    break;

                case "Category":
                    if (sortByAscending)
                        query = context.Posts
                                        .OrderBy(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    else
                        query = context.Posts
                                        .OrderByDescending(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Include(p => p.Category);
                    break;

                default:
                    query = context.Posts
                                    .OrderByDescending(p => p.PostedOn)
                                    .Skip(pageNo * pageSize)
                                    .Take(pageSize)
                                    .Include(p => p.Category);
                    break;
            }

            return query.ToList();
        }

        // returns total number of published posts.
        public int TotalPosts(bool checkIsPublished = true)
        {
            return context.Posts
                .Where(p => checkIsPublished || p.Published == true).Count();
        }

        // returns the Id of the added post
        public int AddPost(Post post)
        {
            
            context.Posts.Add(post); //add the post to queue
            if (post.Tags != null)
            {               
                foreach (Tag tag in post.Tags)
                {
                    PostTagMap postTag = new PostTagMap();
                    postTag.Post_id = post.PostId;
                    postTag.Tag_id = tag.TagId;
                    context.PostTagMaps.Add(postTag);
                    context.SaveChanges();
                }
            }
            context.SaveChanges(); // save changes in queue
            return post.PostId;
        }

        // returns a searchable list of categories
        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }

        // method that saves the edited changes in a post
        public void EditPost(Post post)
        {
            post.Modified = DateTime.Now; // updates modified datetime
            context.Posts.Attach(post); // adds post to queue.
            context.Entry(post).State = EntityState.Modified; //makes post's state set to modified
            context.SaveChanges(); // saves changes in queue
        }

        // method that deletes the selected post
        public void DeletePost(int postId)
        {
            Post dbEntry = context.Posts.Find(postId); //find post based on postId and assign it to dbEntry
            if (dbEntry != null)
            {
                context.Posts.Remove(dbEntry); //add post to queue for deletion
                context.SaveChanges(); // save changes in queue.
            }
        }

        // returns the Id of the added category
        public int AddCategory(Category category)
        {
            context.Categories.Add(category); // adds category to queue
            context.SaveChanges(); // saves changes in queue.
            return category.CategoryId;      
        }

        // method that saves the edited changes in a category
        public void EditCategory(Category category)
        {
            context.Categories.Attach(category); // add category to queue
            context.Entry(category).State = EntityState.Modified; // makes category's state set to modified
            context.SaveChanges(); // save changes in queue
        }

        // method that deletes the selected category and all the posts linked to the category
        public void DeleteCategory(int categoryId)
        {
            Category dbEntry = context.Categories.Find(categoryId); // find category based on categoryId and assign in to dbEntry
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry); // add category to queue for deletion

                // for each post associated with the selected category, add it to queue for deletion
                foreach (Post p in context.Posts.Where(p => p.CategoryId == dbEntry.CategoryId))
                {
                    context.Posts.Remove(p);
                }
                context.SaveChanges(); // save changes in queue.
            }
        }

        public IQueryable<Tag> Tags
        {
            get { return context.Tags; }
        }

        public IQueryable<PostTagMap> PostTagMap
        {
            get { return context.PostTagMaps; }
        }

        // returns the Id of the added tag
        public int AddTag(Tag tag)
        {
            context.Tags.Add(tag); // adds category to queue
            context.SaveChanges(); // saves changes in queue.
            return tag.TagId;
        }

        // method that saves the edited changes in a tag
        public void EditTag(Tag tag)
        {
            context.Tags.Attach(tag); // add category to queue
            context.Entry(tag).State = EntityState.Modified; // makes category's state set to modified
            context.SaveChanges(); // save changes in queue
        }

        // method that deletes the selected tag and all the rows in PostTagMap that contain selected tag
        public void DeleteTag(int tagId)
        {
            Tag dbEntry = context.Tags.Find(tagId); // find category based on tagId and assign in to dbEntry
            if (dbEntry != null)
            {
                context.Tags.Remove(dbEntry); // add tag to queue for deletion

                // for each PostTagMap associated with the selected tag, add it to queue for deletion
                foreach (PostTagMap p in context.PostTagMaps.Where(p => p.Tag_id == dbEntry.TagId))
                {
                    context.PostTagMaps.Remove(p);
                }
                context.SaveChanges(); // save changes in queue.
            }
        }
    }
}
