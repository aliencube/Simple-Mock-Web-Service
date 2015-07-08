using System.Linq;
using Aliencube.SimpleMock.Configs.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace Aliencube.SimpleMock.Configs.Tests
{
    /// <summary>
    /// This represents the test entity for the configuration settings.
    /// </summary>
    [TestFixture]
    public class SimpleMockSettingsTest
    {
        private ISimpleMockSettings _settings;

        [SetUp]
        public void Init()
        {
            this._settings = SimpleMockSettings.CreateInstance();
        }

        [TearDown]
        public void Dispose()
        {
            this._settings.Dispose();
        }

        [Test]
        [TestCase("api", "GET,POST,PUT,DELETE", "json,js,txt")]
        public void GlobalSettings_ShouldReturnValues(string prefix, string verbs, string jsonFileExtensions)
        {
            var globalSettings = this._settings.GlobalSettings;
            globalSettings.WebApiPrefix.Should().Be(prefix);
            globalSettings.Verbs.Should().BeEquivalentTo(verbs.Split(',').ToList());
            globalSettings.JsonFileExtensions.Should().BeEquivalentTo(jsonFileExtensions.Split(',').ToList());
        }

        [Test]
        [TestCase("Content")]
        public void ApiGroup_ShouldReturnValue(string key)
        {
            var apiGroups = this._settings.ApiGroups;
            apiGroups.Should().HaveCount(1);

            var apiGroup = apiGroups.Cast<ApiGroupElement>().First();
            apiGroup.Key.Should().Be(key);
        }

        [Test]
        [TestCase(0, "GetContents", "Content", "GET", "/api/contents", "~/responses/get.contents.json", null)]
        [TestCase(1, "GetContentById", "", "GET", "/api/content/1", "~/responses/get.content.1.json", 0)]
        [TestCase(2, "PostContent", "", "POST", "/api/content", "", 0)]
        [TestCase(3, "PutContentById", "", "PUT", "/api/content/1", "~/responses/put.content.1.json", 0)]
        [TestCase(4, "DeleteContentById", "", "DELETE", "/api/content/1", "~/responses/delete.content.1.json", null)]
        public void Api_Should_ReturnValues(int index, string key, string group, string method, string url, string src, int? delay)
        {
            var apiGroups = this._settings.ApiGroups;
            apiGroups.Should().HaveCount(1);

            var apiGroup = apiGroups.Cast<ApiGroupElement>().First();
            var api = apiGroup.Apis[index];

            api.Key.ToLower().Should().Be(key.ToLower());
            api.Group.ToLower().Should().Be(group.ToLower());
            api.Method.ToUpper().Should().Be(method.ToUpper());
            api.Url.ToLower().Should().Be(url.ToLower());
            api.Src.ToLower().Should().Be(src.ToLower());
            api.Delay.Should().Be(delay.GetValueOrDefault());
        }
    }
}