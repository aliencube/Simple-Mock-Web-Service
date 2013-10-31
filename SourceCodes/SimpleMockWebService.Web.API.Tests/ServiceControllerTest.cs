using NUnit.Framework;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using SimpleMockWebService.Web.API.Controllers;
using System;
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

            this._controller = new ServiceController(this._settings, this._service);
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
        [TestCase("get", "/api/contents", "", 200)]
        [TestCase("get", "/api/content/1", "", 200)]
        [TestCase("post", "/api/content", "={ \"id\": 1 }", 200)]
        [TestCase("put", "/api/content/1", "={ \"id\": 1 }", 200)]
        [TestCase("put", "/api/content/1", "", 415)]
        [TestCase("delete", "/api/content/1", "", 200)]
        [TestCase("head", "/api/content/1", "", 405)]
        [TestCase("get", "/content/1", "", 404)]
        [TestCase("get", "/content/not-found", "", 404)]
        public void GetHttpResponseMessage_SendMethodAndUrl_MessageReturned(string method, string url, string value, int statusCode)
        {
            var httpMethod = Enum.Parse(typeof(HttpMethod), method, true) as HttpMethod;
            using (var request = new HttpRequestMessage(httpMethod, "http://localhost"))
            {
                if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
                {
                    request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    request.Headers.Add("Content-Length", Convert.ToString(value.Length));
                }
                this._controller.ControllerContext = new HttpControllerContext(this._config, this._routeData, request);
                this._controller.Request = request;
                using (var response = httpMethod == HttpMethod.Get
                                          ? this._controller.Get()
                                          : (httpMethod == HttpMethod.Post
                                                 ? this._controller.Post(value)
                                                 : (httpMethod == HttpMethod.Put
                                                        ? this._controller.Put(value)
                                                        : this._controller.Delete())))
                {
                    Assert.AreEqual(statusCode, response.StatusCode);
                }
            }
        }

        #endregion Tests
    }
}