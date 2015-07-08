using System.Configuration;

namespace Aliencube.SimpleMock.Configs
{
    /// <summary>
    /// This represents the API element entity.
    /// </summary>
    public class ApiGroupElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the key for unique identification.
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of API elements.
        /// </summary>
        [ConfigurationProperty("apis", IsRequired = true)]
        public ApiElementCollection Apis
        {
            get { return (ApiElementCollection)this["apis"]; }
            set { this["apis"] = value; }
        }
    }
}