using NUnit.Framework;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using System.Configuration;
using System.Linq;

namespace SimpleMockWebService.Tests
{
    /// <summary>
    /// This represents the test entity for the configuration settings.
    /// </summary>
    [TestFixture]
    public class ConfigurationTest
    {
        private ISimpleMockWebServiceSettings _settings;

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            this._settings = ConfigurationManager.GetSection("simpleMockWebService") as SimpleMockWebServiceSettings;
        }

        [TearDown]
        public void Dispose()
        {
            this._settings.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        /// <summary>
        /// Tests to get the list of api keys from the configuration settings.
        /// </summary>
        [Test]
        [TestCase("api")]
        public void GetGlobalSettings_SendConfigurationSettings_GlobalSettingsReturned(string prefix)
        {
            var result = this._settings.GlobalSettings.WebApiPrefix;
            Assert.AreEqual(prefix, result);
        }

        /// <summary>
        /// Tests to get the list of api keys from the configuration settings.
        /// </summary>
        [Test]
        public void GetListsOfApis_SendConfigurationSettings_ListOfApisReturned()
        {
            var keys = this._settings
                           .ApiGroups
                           .Cast<ApiGroupElement>()
                           .First()
                           .Apis
                           .Cast<ApiElement>()
                           .Select(p => p.Key)
                           .ToList();
            Assert.IsTrue(keys != null && keys.Any());
        }

        #endregion Tests
    }
}