﻿using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Moq;
using NUnit.Framework;
using Should;
using StructureMap;
using WebApiContrib.IoC.StructureMap.Tests.Helpers;

namespace WebApiContrib.IoC.StructureMap.Tests.IoC
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        [Test]
        public void StructureMapResolver_should_resolve_registered_ContactRepository()
        {
            var resolver = new StructureMapResolver(new Container(x =>
                x.For<IContactRepository>().Use<InMemoryContactRepository>()
            ));

            var instance = resolver.GetService(typeof(IContactRepository));

            Assert.IsNotNull(instance);
        }

        [Test]
        public void StructureMapResolver_should_not_resolve_unregistered_ContactRepository()
        {
            var resolver = new StructureMapResolver(new Container());

            var instance = resolver.GetService(typeof(IContactRepository));

            Assert.IsNull(instance);
        }

        [Test]
        public void StructureMapResolver_should_resolve_registered_ContactRepository_through_host()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            var container = new Container(x => x.For<IContactRepository>().Use<InMemoryContactRepository>());
            config.DependencyResolver = new StructureMapResolver(container);

            var server = new HttpServer(config);
            var client = new HttpClient(server);

            var response = client.GetAsync("http://anything/api/contacts").Result;
            Assert.IsNotNull(response.Content);
        }

        [Test]
        public void StructureMapResolver_should_return_both_instaces_of_IContactRepository()
        {
            var container = new Container(x => x.Scan(s =>
            {
                s.TheCallingAssembly();
                s.AddAllTypesOf<IContactRepository>().NameBy(t => t.Name);
            }));
            var config = new HttpConfiguration();
            var resolver = new StructureMapResolver(container);

            config.DependencyResolver = resolver;
            var repositories = config.DependencyResolver.GetServices(typeof(IContactRepository));

            repositories.Count().ShouldEqual(2);
        }

        [Test]
        public void StructureMapResolver_should_return_an_empty_collection_if_type_isnt_found()
        {
            var config = new HttpConfiguration();
            var resolver = new StructureMapResolver(new Container());

            config.DependencyResolver = resolver;
            var repositories = config.DependencyResolver.GetServices(typeof(IContactRepository));

            repositories.Count().ShouldEqual(0);
        }

        [Test]
        public void StructureMapResolver_should_not_dispose_container()
        {
            var mockContainer = new Mock<IContainer>();

            var resolver = new StructureMapResolver(mockContainer.Object);

            resolver.Dispose();

            mockContainer.Verify(c => c.Dispose(), Times.Never, "Should not have called Dispose on the container.");
        }

        [Test]
        public void StructureMapResolver_should_throw_when_attempting_to_get_service_after_being_disposed()
        {
            var mockContainer = new Mock<IContainer>();

            var resolver = new StructureMapResolver(mockContainer.Object);

            resolver.Dispose();

            Assert.That(() => resolver.GetService(typeof (IContactRepository)),
                Throws.Exception.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void StructureMapResolver_should_throw_when_attempting_to_get_services_after_being_disposed()
        {
            var mockContainer = new Mock<IContainer>();

            var resolver = new StructureMapResolver(mockContainer.Object);

            resolver.Dispose();

            Assert.That(() => resolver.GetServices(typeof(IContactRepository)),
                Throws.Exception.TypeOf<ObjectDisposedException>());
        }
    }
}
