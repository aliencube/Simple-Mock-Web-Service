using System;
using NUnit.Framework;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using SimpleMockWebService.Web.API.Controllers;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace SimpleMockWebService.Web.API.Tests
{
    /// <summary>
    /// This represents the entity to test the service Web API controller.
    /// </summary>
    [TestFixture]
    public class ServiceControllerTest
    {
        private ISimpleMockWebServiceSettings _settings;
        private IMockService _service;
        private ServiceController _controller;
        private HttpConfiguration _config;
        private HttpRouteData _routeData;

        #region SetUp / TearDown

        /// <summary>
        /// Initialises test envirnments before any test in the test fixture is run.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            this._settings = ConfigurationManager.GetSection("simpleMockWebService") as SimpleMockWebServiceSettings;
            this._service = new MockService(this._settings);

            this._config = new HttpConfiguration();
            var route = this._config.Routes.MapHttpRoute("DefaultApi",
                                                         "api/{controller}/{id}",
                                                         new { id = RouteParameter.Optional });
            this._routeData = new HttpRouteData(route,
                                              new HttpRouteValueDictionary() { { "controller", "Service" } });

            this._controller = new ServiceController();
            //this._controller = new ServiceController(this._settings, this._service);
            this._controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = this._config;
        }

        /// <summary>
        /// Releases test environments after all the tests in the test fixture have been run.
        /// The method is guaranteed to be called, even if an exception is thrown.
        /// </summary>
        [TestFixtureTearDown]
        public void Dispose()
        {
            this._controller.Dispose();
            this._config.Dispose();
            this._service.Dispose();
            this._settings.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase("/api/contents", true)]
        [TestCase("/api/content/1", true)]
        public void GetHttpResponseMessage_SendGetMethod_MessageReturned(string url, bool exists)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get,
                                                        String.Format("http://localhost{0}", url)))
            {
                this._controller.ControllerContext = new HttpControllerContext(this._config, this._routeData, request);
                this._controller.Request = request;
                using (var response = this._controller.Get())
                {
                }
            }
        }

        #endregion Tests
    }
}