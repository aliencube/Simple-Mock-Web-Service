using System.Reflection;
using System.Web.Http;
using Aliencube.SimpleMock.Configs;
using Aliencube.SimpleMock.Configs.Interfaces;
using Aliencube.SimpleMock.Services;
using Aliencube.SimpleMock.Services.Interfaces;
using Autofac;
using Autofac.Integration.WebApi;

namespace Aliencube.SimpleMock.WebApi
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            RegisterSettings(builder);
            RegisterServices(builder);
            RegisterControllers(builder);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static void RegisterSettings(ContainerBuilder builder)
        {
            builder.Register(p => SimpleMockSettings.CreateInstance()).As<ISimpleMockSettings>().PropertiesAutowired();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<MockService>().As<IMockService>().PropertiesAutowired();
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}