using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.SelfHost;
using SimpleMockWebService.Web.API;
using SimpleMockWebService.IoC.App_Start;

namespace SimpleMockWebService.Web.API.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:8080";
            var config = new HttpSelfHostConfiguration(url);

            config.Routes.MapHttpRoute(
                "SimpleMockWebService", "{*param}",
                new { controller = "Service", param = RouteParameter.Optional });

            UnityWebApiActivator.StartWithDependencyResolver(config);

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("SimpleMockWebService is On... {0}", url);
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
