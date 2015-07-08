using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Aliencube.ConfigurationConverters;

namespace Aliencube.SimpleMock.Configs
{
    /// <summary>
    /// This represents the global settings element entity.
    /// </summary>
    public class GlobalSettingsElement : ConfigurationElement
    {
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
                {
                    value = value.Substring(0, value.Length - 1);
                }
                return value;
            }
            set { this["webApiPrefix"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of method verbs for RESTful web service delimited with comma.
        /// Default value is <c>GET,POST,PUT,DELETE</c>.
        /// </summary>
        [ConfigurationProperty("verbs", DefaultValue = "GET,POST,PUT,DELETE", IsRequired = false)]
        [TypeConverter(typeof(CommaDelimitedListConverter<string>))]
        public List<string> Verbs
        {
            get { return (List<string>)this["verbs"]; }
            set { this["verbs"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of JSON file extensions delimited with comma.
        /// Default value is <c>json,js,txt</c>.
        /// </summary>
        [ConfigurationProperty("jsonFileExtensions", DefaultValue = "json,js,txt", IsRequired = false)]
        [TypeConverter(typeof(CommaDelimitedListConverter<string>))]
        public List<string> JsonFileExtensions
        {
            get { return (List<string>)this["jsonFileExtensions"]; }
            set { this["jsonFileExtensions"] = value; }
        }
    }
}