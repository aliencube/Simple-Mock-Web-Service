using System.Configuration;

namespace SimpleMockWebService.Configurations
{
    /// <summary>
    /// This represents the global settings element entity.
    /// </summary>
    public class GlobalSettingsElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets or sets the prefix used for Web API.
        /// If the value ends with "/", the return value will omit it.
        /// </summary>
        [ConfigurationProperty("webApiPrefix", DefaultValue = "api", IsRequired = false)]
        public string WebApiPrefix
        {
            get
            {
                var value = (string)this["webApiPrefix"];
                if (value.EndsWith("/"))
                    value = value.Substring(0, value.Length - 1);
                return value;
            }
            set { this["webApiPrefix"] = value; }
        }

        #endregion Properties
    }
}