using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Hosting;
using Aliencube.SimpleMock.Configs;
using Aliencube.SimpleMock.Configs.Interfaces;
using Aliencube.SimpleMock.Services.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace Aliencube.SimpleMock.Services.Tests
{
    [TestFixture]
    public class MockServiceTest
    {
        private ISimpleMockSettings _settings;
        private IMockService _service;
        private HttpConfiguration _config;
        private Regex _regexSourcePath;

        [SetUp]
        public void Init()
        {
            this._settings = SimpleMockSettings.CreateInstance();
            this._service = new MockService(this._settings);
            this._config = new HttpConfiguration();
            this._regexSourcePath = new Regex(@"^[A-Z]+:\\.+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        [TearDown]
        public void Cleanup()
        {
            if (this._service != null)
            {
                this._service.Dispose();
            }

            if (this._config != null)
            {
                this._config.Dispose();
            }

            if (this._settings != null)
            {
                this._settings.Dispose();
            }
        }

        [Test]
        [TestCase("get", false)]
        [TestCase("post", true)]
        [TestCase("put", true)]
        [TestCase("delete", false)]
        public void IsRequestBodyRequired_SendMethod_ResultReturned(string method, bool expected)
        {
            var httpMethod = new HttpMethod(method);
            using (var request = new HttpRequestMessage(httpMethod, "http://localhost"))
            {
                var required = this._service.IsRequestBodyRequired(request);
                required.Should().Be(expected);
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
            validated.Should().Be(expected);
        }

        [Test]
        [TestCase("contents", false)]
        [TestCase("/api/contents", true)]
        [TestCase("~/api/content/1", true)]
        public void IsValidPrefix_SendUrl_ResultReturned(string url, bool expected)
        {
            var validated = this._service.IsValidPrefix(url);
            validated.Should().Be(expected);
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
            validated.Should().Be(expected);
        }

        [Test]
        [TestCase("get", "/api/contents")]
        [TestCase("get", "/api/content/1")]
        [TestCase("post", "/api/content")]
        [TestCase("put", "/api/content/1")]
        [TestCase("delete", "/api/content/1")]
        public void GetApiElement_SendMethodAndUrl_ApiElementReturned(string method, string url)
        {
            var api = this._service.GetApiElement(method, url);
            api.Should().NotBeNull();
        }

        [Test]
        [TestCase("/api/contents", 1)]
        [TestCase("/api/content/1", 2)]
        public void GetApiUrlSegments_SendUrl_SegmentsReturned(string url, int count)
        {
            var segments = this._service.GetApiUrlSegments(url);
            segments.Should().HaveCount(count);
        }

        [Test]
        [TestCase("/api/contents", "contents")]
        [TestCase("/api/content/1", "content")]
        public void GetApiController_SendUrl_ControllerReturned(string url, string expected)
        {
            var controller = this._service.GetApiController(url);
            controller.Should().Be(expected);
        }

        [Test]
        [TestCase("/api/contents", 0)]
        [TestCase("/api/content/1", 1)]
        [TestCase("/api/content/1/title", 2)]
        public void GetApiParameters_SendUrl_ParametersReturned(string url, int count)
        {
            var parameters = this._service.GetApiParameters(url);
            var result = parameters != null ? parameters.Count : 0;
            result.Should().Be(count);
        }

        [Test]
        [TestCase("get", "/api/contents", "~/responses/get.contents.json")]
        [TestCase("get", "/api/content/1", "~/responses/get.content.1.json")]
        [TestCase("get", "/api/content/1/title", "~/responses/get.content.1.title.json")]
        [TestCase("post", "/api/content", "~/responses/post.content.json")]
        [TestCase("put", "/api/content/1", "~/responses/put.content.1.json")]
        [TestCase("delete", "/api/content/1", "~/responses/delete.content.1.json")]
        public void GetDefaultApiResponseFilePath_SendMethodAndUrl_ResponseFilePathReturned(string method,
                                                                                            string url,
                                                                                            string expected)
        {
            var filepath = this._service.GetDefaultApiResponseFilePath(method, url);
            filepath.ToLower().Should().Be(expected.ToLower());
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
            this._regexSourcePath.IsMatch(fullpath).Should().BeTrue();
            fullpath.ToLower().EndsWith(filename.ToLower()).Should().BeTrue();
        }

        [Test]
        [TestCase("get", "/api/contents")]
        [TestCase("get", "/api/content/1")]
        [TestCase("post", "/api/content")]
        [TestCase("put", "/api/content/1")]
        [TestCase("delete", "/api/content/1")]
        public async void GetApiResponse_SendSrc_JsonResponseReturned(string method, string url)
        {
            var fullpath = this._service.GetApiReponseFullPath(method, url);
            var response = await this._service.GetApiResponseAsync(fullpath);

            response.Should().NotBeNullOrWhiteSpace();
            response.ToCharArray().First().ToString().Should().BeOneOf(new string[] {"[", "{"});
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
        public async void GetHttpResponse_SendMethodAndUrl_JsonResponseReturned(string method, string url, string value, int statusCode)
        {
            var httpMethod = new HttpMethod(method);
            using (var request = new HttpRequestMessage(httpMethod, String.Format("http://localhost?url={0}", url)))
            {
                request.Properties[HttpPropertyKeys.HttpConfigurationKey] = this._config;
                var response = await this._service.GetHttpResponseAsync(request, value);
                Convert.ToInt32(response.StatusCode).Should().Be(statusCode);
            }
        }
    }
}