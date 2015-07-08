using System.Web.Http;

namespace Aliencube.SimpleMock.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            DependencyConfig.RegisterDependencies();
        }
    }
}
