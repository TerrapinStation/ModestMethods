using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System.Configuration;
using ModestMethods.Domain.Abstract;
using ModestMethods.Domain.Concrete;

namespace EssentialTools.Infrastructure
{
    // using ninject to bind the interface(IBlogRepository), leveraged by the UI, to the entity models and methods defined in EFBlogRepository
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IBlogRepository>().To<EFBlogRepository>();
        }
    }
}
