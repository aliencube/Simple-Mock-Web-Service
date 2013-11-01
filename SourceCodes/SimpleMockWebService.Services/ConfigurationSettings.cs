using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services.Interfaces;
using System.Configuration;

namespace SimpleMockWebService.Services
{
    /// <summary>
    /// This represents the configuration settings from web.config/app.config entity.
    /// </summary>
    public class ConfigurationSettings : IConfigurationSettings
    {
        #region Properties

        private ISimpleMockWebServiceSettings _simpleMockWebServiceSettings;

        /// <summary>
        /// Gets the SimpleMockWebService element section.
        /// </summary>
        public ISimpleMockWebServiceSettings SimpleMockWebServiceSettings
        {
            get
            {
                if (this._simpleMockWebServiceSettings == null)
                    this._simpleMockWebServiceSettings = ConfigurationManager.GetSection("simpleMockWebService") as SimpleMockWebServiceSettings;
                return this._simpleMockWebServiceSettings;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Methods
    }
}