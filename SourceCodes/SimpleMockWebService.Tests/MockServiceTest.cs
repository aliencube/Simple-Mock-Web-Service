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

        #endregion Tests
    }
}