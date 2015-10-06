# WebApiContrib.IoC.StructureMap

This gives you all the pieces you need to hook up StructureMap as your Inversion of Control container for ASP.NET Web API.

## Install

The best way to get your hands on [WebApiContrib.IoC.StructureMap](/) is to grab the package from NuGet:

    Install-Package WebApiContrib.IoC.StructureMap

## How to Use

Using the WebApiContrib StructureMap container is easy.

    var config = GlobalConfiguration.Configuration;
    var container = GetYourOwnBuiltContainer(); // You build this up yourself.
    config.DependencyResolver = new StructureMapResolver(container);

One thing that is unique about this resolver is that it is also an instance of IHttpControllerActivator. Upon creation of the StructureMapResolver, it injects itself as the object to be returned when IHttpControllerActivator is requested.

## Help! - Issues, Questions, and Anything Else

If you're in need of any assistance, your fastest method to get in touch with somebody that might be able to help is through the WebApiContrib mailing list: https://groups.google.com/group/webapicontrib

Another great way is to [leave an issue](https://github.com/WebApiContrib/WebApiContrib.IoC.StructureMap/issues) on the GitHub repository.

For StructureMap specific questions, please see the [StructureMap documentation](http://www.structuremap.net).
