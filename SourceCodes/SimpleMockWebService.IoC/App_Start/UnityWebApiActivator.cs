using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SimpleMockWebService.IoC.App_Start.UnityWebApiActivator), "Start")]

namespace SimpleMockWebService.IoC.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityWebApiActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>Integrates Unity when the application starts on SelfHost environment.</summary>
        public static void StartWithDependencyResolver(HttpConfiguration config)
        {
            var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            config.DependencyResolver = resolver;
        }
    }
}
