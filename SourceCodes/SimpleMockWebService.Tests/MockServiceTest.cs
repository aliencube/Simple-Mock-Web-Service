using NUnit.Framework;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using System.Collections.Generic;
using System.Configuration;
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
        [TestCase("~/api/content/1", true)]
        [TestCase("/api/contents", true)]
        [TestCase("contents", false)]
        public void IsValidPrefix_SendUrl_ResultReturned(string url, bool expected)
        {
            var validated = this._service.IsValidPrefix(url);
            Assert.AreEqual(expected, validated);
        }

        [Test]
        [TestCase("get", true)]
        [TestCase("head", false)]
        public void IsValidMethod_SendMethod_ResultReturned(string method, bool expected)
        {
            var validated = this._service.IsValidMethod(method);
            Assert.AreEqual(expected, validated);
        }

        [Test]
        [TestCase("~/responses/get.contents.json", true)]
        [TestCase("", false)]
        [TestCase("/responses/put.content.1.js", true)]
        [TestCase("~/responses/get.contents.json", true)]
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
        public void GetApiElement_SendMethodAndUrl_ApiElementReturned(string method, string url, bool expected)
        {
            var api = this._service.GetApiElement(method, url);
            Assert.AreEqual(expected, api != null);
        }

        [Test]
        [TestCase("/api/content/1", 2)]
        [TestCase("/api/contents", 1)]
        public void GetApiUrlSegments_SendUrl_SegmentsReturned(string url, int count)
        {
            var segments = this._service.GetApiUrlSegments(url);
            Assert.AreEqual(count, segments.Count);
        }

        [Test]
        [TestCase("/api/content/1", "content")]
        [TestCase("/api/contents", "contents")]
        public void GetApiController_SendUrl_ControllerReturned(string url, string expected)
        {
            var controller = this._service.GetApiController(url);
            Assert.AreEqual(expected, controller);
        }

        [Test]
        [TestCase("/api/content/1/title", 2)]
        [TestCase("/api/content/1", 1)]
        [TestCase("/api/contents", 0)]
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
        [TestCase("put", "/api/content/1", "~/responses/put.content.1.json")]
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
        public void GetApiReponseFullPath_SendSrc_FullPathReturned(string method, string url, string filename)
        {
            var api = this._service.GetApiElement(method, url);
            var fullpath = this._service.GetApiReponseFullPath(api);
            Assert.IsTrue(this._regexSourcePath.IsMatch(fullpath));
            Assert.IsTrue(fullpath.ToLower().EndsWith(filename.ToLower()));
        }

        [Test]
        [TestCase("get", "/api/contents")]
        [TestCase("get", "/api/content/1")]
        [TestCase("post", "/api/content")]
        public void GetApiResponse_SendSrc_JsonResponseReturned(string method, string url)
        {
            var api = this._service.GetApiElement(method, url);
            var fullpath = this._service.GetApiReponseFullPath(api);
            var response = this._service.GetApiResponse(fullpath);
            Assert.IsTrue(response.StartsWith("[") || response.StartsWith("{"));
        }

        [Test]
        [TestCase("get", "/api/contents")]
        [TestCase("get", "/api/content/1")]
        [TestCase("post", "/api/content")]
        public void GetResponse_SendMethodAndUrl_JsonResponseReturned(string method, string url)
        {
            var api = this._service.GetApiElement(method, url);
            var fullpath = this._service.GetApiReponseFullPath(api);
            var items = new Dictionary<string, string>()
                        {
                            {"method", method},
                            {"url", url},
                            {"src", fullpath}
                        };
            var response = this._service.GetResponse(items);
            Assert.IsTrue(response.StartsWith("[") || response.StartsWith("{"));
        }

        #endregion Tests
    }
}