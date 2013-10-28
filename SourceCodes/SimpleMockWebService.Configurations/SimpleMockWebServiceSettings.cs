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
        /// Gets or sets the collection of API elements.
        /// </summary>
        [ConfigurationProperty("apis", IsRequired = true)]
        public ApiCollection Apis
        {
            get { return (ApiCollection)this["apis"]; }
            set { this["apis"] = value; }
        }

        #endregion Properties
    }
}