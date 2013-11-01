using NUnit.Framework;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using SimpleMockWebService.Web.API.Controllers;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using ConfigurationSettings = SimpleMockWebService.Services.ConfigurationSettings;

namespace SimpleMockWebService.Web.API.Tests
{
    /// <summary>
    /// This represents the entity to test the service Web API controller.
    /// </summary>
    [TestFixture]
    public class ServiceControllerTest
    {
        private IConfigurationSettings _settings;
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
            this._settings = new ConfigurationSettings();
            this._service = new MockService(this._settings);

            this._config = new HttpConfiguration();
            var route = this._config.Routes.MapHttpRoute("DefaultApi",
                                                         "api/{controller}/{id}",
                                                         new { id = RouteParameter.Optional });
            this._routeData = new HttpRouteData(route,
                                                new HttpRouteValueDictionary()
                                                {
                                                    { "controller", "Service" }
                                                });

            this._controller = new ServiceController(this._settings, this._service);
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
            var httpMethod = new HttpMethod(method);
            using (var request = new HttpRequestMessage(httpMethod, String.Format("http://localhost?url={0}", url)))
            {
                request.Properties[HttpPropertyKeys.HttpConfigurationKey] = this._config;
                this._controller.Request = request;
                this._controller.ControllerContext = new HttpControllerContext(this._config, this._routeData, request);
                using (var response = httpMethod == HttpMethod.Get
                                          ? this._controller.Get()
                                          : (httpMethod == HttpMethod.Post
                                                 ? this._controller.Post(value)
                                                 : (httpMethod == HttpMethod.Put
                                                        ? this._controller.Put(value)
                                                        : this._controller.Delete())))
                {
                    Assert.AreEqual(statusCode, Convert.ToInt32(response.StatusCode));
                }
            }
        }

        #endregion Tests
    }
}