using NUnit.Framework;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SimpleMockWebService.Tests
{
    [TestFixture]
    public class MockServiceTest
    {
        private ISimpleMockWebServiceSettings _settings;
        private IMockService _service;
        private Regex _regexSourcePath;

        #region SetUp / TearDown

        [TestFixtureSetUp]
        public void Init()
        {
            this._settings = ConfigurationManager.GetSection("simpleMockWebService") as SimpleMockWebServiceSettings;
            this._service = new MockService(this._settings);
            this._regexSourcePath = new Regex(@"^[A-Z]+:\\.+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            this._service.Dispose();
            this._settings.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase("get", false)]
        [TestCase("post", true)]
        [TestCase("put", true)]
        [TestCase("delete", false)]
        public void IsRequestBodyRequired_SendMethod_ResultReturned(string method, bool expected)
        {
            var httpMethod = Enum.Parse(typeof(HttpMethod), method, true) as HttpMethod;
            using (var request = new HttpRequestMessage(httpMethod, "http://localhost{0}"))
            {
                var required = this._service.IsRequestBodyRequired(request);
                Assert.AreEqual(expected, required);
            }
        }

        [Test]
        [TestCase("get", true)]
        [TestCase("post", true)]
        [TestCase("put", true)]
        [TestCase("delete", true)]
        [TestCase("head", false)]
        public void IsValidMethod_SendMethod_ResultReturned(string method, bool expected)
        {
            var validated = this._service.IsValidMethod(method);
            Assert.AreEqual(expected, validated);
        }

        [Test]
        [TestCase("contents", false)]
        [TestCase("/api/contents", true)]
        [TestCase("~/api/content/1", true)]
        public void IsValidPrefix_SendUrl_ResultReturned(string url, bool expected)
        {
            var validated = this._service.IsValidPrefix(url);
            Assert.AreEqual(expected, validated);
        }

        [Test]
        [TestCase("", false)]
        [TestCase("~/responses/get.contents.json", true)]
        [TestCase("~/responses/get.content.1.json", true)]
        [TestCase("~/responses/post.content.json", true)]
        [TestCase("/responses/put.content.1.js", true)]
        [TestCase("~/responses/delete.content.1.txt", true)]
        public void HasValidJsonFileExtension_SendFilePath_ResultReturned(string src, bool expected)
        {
            var validated = this._service.HasValidJsonFileExtension(src);
            Assert.AreEqual(expected, validated);
        }

        [Test]
        [TestCase("get", "/api/contents", true)]
        [TestCase("get", "/api/content/1", true)]
        [TestCase("post", "/api/content/1", false)]
        [TestCase("put", "/api/content/1", true)]
        [TestCase("delete", "/api/content/1", true)]
        public void GetApiElement_SendMethodAndUrl_ApiElementReturned(string method, string url, bool expected)
        {
            var api = this._service.GetApiElement(method, url);
            Assert.AreEqual(expected, api != null);
        }

        [Test]
        [TestCase("/api/contents", 1)]
        [TestCase("/api/content/1", 2)]
        public void GetApiUrlSegments_SendUrl_SegmentsReturned(string url, int count)
        {
            var segments = this._service.GetApiUrlSegments(url);
            Assert.AreEqual(count, segments.Count);
        }

        [Test]
        [TestCase("/api/contents", "contents")]
        [TestCase("/api/content/1", "content")]
        public void GetApiController_SendUrl_ControllerReturned(string url, string expected)
        {
            var controller = this._service.GetApiController(url);
            Assert.AreEqual(expected, controller);
        }

        [Test]
        [TestCase("/api/contents", 0)]
        [TestCase("/api/content/1", 1)]
        [TestCase("/api/content/1/title", 2)]
        public void GetApiParameters_SendUrl_ParametersReturned(string url, int count)
        {
            var parameters = this._service.GetApiParameters(url);
            var result = parameters != null ? parameters.Count : 0;
            Assert.AreEqual(count, result);
        }

        [Test]
        [TestCase("get", "/api/contents", "~/responses/get.contents.json")]
        [TestCase("get", "/api/content/1", "~/responses/get.content.1.json")]
        [TestCase("get", "/api/content/1/title", "~/responses/get.content.1.title.json")]
        [TestCase("post", "/api/content", "~/response/post.content.json")]
        [TestCase("put", "/api/content/1", "~/responses/put.content.1.json")]
        [TestCase("delete", "/api/content/1", "~/responses/delete.content.1.json")]
        public void GetDefaultApiResponseFilePath_SendMethodAndUrl_ResponseFilePathReturned(string method,
                                                                                            string url,
                                                                                            string expected)
        {
            var filepath = this._service.GetDefaultApiResponseFilePath(method, url);
            Assert.AreEqual(expected.ToLower(), filepath.ToLower());
        }

        [Test]
        [TestCase("get", "/api/contents", "get.contents.json")]
        [TestCase("get", "/api/content/1", "get.content.1.json")]
        [TestCase("post", "/api/content", "post.content.json")]
        [TestCase("put", "/api/content/1", "put.content.1.json")]
        [TestCase("delete", "/api/content/1", "delete.content.1.json")]
        public void GetApiReponseFullPath_SendSrc_FullPathReturned(string method, string url, string filename)
        {
            var fullpath = this._service.GetApiReponseFullPath(method, url);
            Assert.IsTrue(this._regexSourcePath.IsMatch(fullpath));
            Assert.IsTrue(fullpath.ToLower().EndsWith(filename.ToLower()));
        }

        [Test]
        [TestCase("get", "/api/contents")]
        [TestCase("get", "/api/content/1")]
        [TestCase("post", "/api/content")]
        [TestCase("put", "/api/content/1")]
        [TestCase("delete", "/api/content/1")]
        public void GetApiResponse_SendSrc_JsonResponseReturned(string method, string url)
        {
            var fullpath = this._service.GetApiReponseFullPath(method, url);
            var response = this._service.GetApiResponse(fullpath);
            Assert.IsTrue(!String.IsNullOrWhiteSpace(response) && (response.StartsWith("[") || response.StartsWith("{")));
        }

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
        public void GetHttpResponse_SendMethodAndUrl_JsonResponseReturned(string method, string url, string value, int statusCode)
        {
            var httpMethod = Enum.Parse(typeof(HttpMethod), method, true) as HttpMethod;
            using (var request = new HttpRequestMessage(httpMethod,
                                                        String.Format("http://localhost{0}", url)))
            {
                var response = this._service.GetHttpResponse(request, value);
                Assert.AreEqual(statusCode, response.StatusCode);
            }
        }

        #endregion Tests
    }
}