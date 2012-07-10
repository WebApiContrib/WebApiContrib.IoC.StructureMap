using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace WebApiContrib.IoC.StructureMap
{
    public class StructureMapDependencyScope : IDependencyScope
    {
        private IContainer container;

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if (container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            return container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed.");

            return container.GetAllInstances(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            container.Dispose();
            container = null;
        }
    }

    public class StructureMapResolver : StructureMapDependencyScope, IDependencyResolver
    {
        private readonly IContainer container;

        public StructureMapResolver(IContainer container)
            : base(container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(container.GetNestedContainer());
        }
    }
}
