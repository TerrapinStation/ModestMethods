#region Usings
using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
#endregion

namespace ModestMethods.WebUI
{
  /// <summary>
  /// Bind POST model to actions.
  /// </summary>
  public class PostModelBinder : DefaultModelBinder
  {
    private readonly IKernel _kernel;

    public PostModelBinder(IKernel kernel)
    {
      _kernel = kernel;
    }

    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var post = (Post)base.BindModel(controllerContext, bindingContext);

      var blogRepository = _kernel.Get<IBlogRepository>();

      if (post.Category != null)
          post.CategoryId = blogRepository.Categories(post.CategoryId)

      if (bindingContext.ValueProvider.GetValue("oper").AttemptedValue.Equals("edit"))
        post.Modified = DateTime.UtcNow; // dates are stored in UTC timezone.
      else
        post.PostedOn = DateTime.UtcNow;

      return post;
    }
  }
}