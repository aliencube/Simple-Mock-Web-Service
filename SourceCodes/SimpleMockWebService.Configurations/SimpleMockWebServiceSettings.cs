using SimpleMockWebService.Configurations.Interfaces;
using System.Configuration;

namespace SimpleMockWebService.Configurations
{
    /// <summary>
    /// This represents the configuration settings entity for Simple Mock Web Service.
    /// </summary>
    public class SimpleMockWebServiceSettings : ConfigurationSection, ISimpleMockWebServiceSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the global settings element.
        /// </summary>
        [ConfigurationProperty("globalSettings", IsRequired = true)]
        public GlobalSettingsElement GlobalSettings
        {
            get { return (GlobalSettingsElement)this["globalSettings"]; }
            set { this["globalSettings"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of API element groups.
        /// </summary>
        [ConfigurationProperty("apiGroups", IsRequired = true)]
        public ApiGroupElementCollection ApiGroups
        {
            get { return (ApiGroupElementCollection)this["apiGroups"]; }
            set { this["apiGroups"] = value; }
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