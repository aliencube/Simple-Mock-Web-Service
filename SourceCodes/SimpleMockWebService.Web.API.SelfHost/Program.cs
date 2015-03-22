using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.SelfHost;
using SimpleMockWebService.Web.API;
using SimpleMockWebService.IoC.App_Start;
using System.Security.Principal;

namespace SimpleMockWebService.Web.API.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                Console.WriteLine("Please run as administrator.");
                Console.ReadLine();
                return;
            }

            var url = "http://localhost:8080";
            var config = new HttpSelfHostConfiguration(url);

            config.Routes.MapHttpRoute(
                "SimpleMockWebService", "{*param}",
                new { controller = "Service", param = RouteParameter.Optional });

            UnityWebApiActivator.StartWithDependencyResolver(config);

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("SimpleMockWebService is UP... {0}", url);
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
