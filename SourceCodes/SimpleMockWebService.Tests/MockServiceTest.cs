using NUnit.Framework;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using System.Configuration;

namespace SimpleMockWebService.Tests
{
    [TestFixture]
    public class MockServiceTest
    {
        private ISimpleMockWebServiceSettings _settings;
        private IMockService _service;

        #region SetUp / TearDown

        [TestFixtureSetUp]
        public void Init()
        {
            this._settings = ConfigurationManager.GetSection("simpleMockWebService") as SimpleMockWebServiceSettings;
            this._service = new MockService(this._settings);
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

        #endregion Tests
    }
}