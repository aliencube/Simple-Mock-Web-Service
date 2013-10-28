using System.Configuration;

namespace SimpleMockWebService.Configurations
{
    /// <summary>
    /// This represents the API element entity.
    /// </summary>
    public class ApiElement : ConfigurationElement
    {
        #region Properties

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
        /// Gets or sets the method verb for the RESTful request.
        /// Default method is <c>GET</c>, if not specified.
        /// </summary>
        [ConfigurationProperty("method", DefaultValue = "GET", IsRequired = false)]
        public string Method
        {
            get { return (string)this["method"]; }
            set { this["method"] = value; }
        }

        /// <summary>
        /// Gets or sets the web API URL.
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }

        /// <summary>
        /// Gets or sets the physical path to return the JSON result as RESTful response.
        /// Default location of the response sources is <c>~/Resources</c>, unless the directory is explicitly specified.
        /// </summary>
        [ConfigurationProperty("src", IsRequired = true)]
        public string Src
        {
            get { return (string)this["src"]; }
            set { this["src"] = value; }
        }

        /// <summary>
        /// Gets or sets the delay time in miliseconds to deliberately delay server response time for simulation purpose.
        /// Default value is <c>0</c>, which doesn't send intentional delay request.
        /// </summary>
        [ConfigurationProperty("delay", DefaultValue = 0, IsRequired = false)]
        public int Delay
        {
            get { return (int)this["delay"]; }
            set { this["delay"] = value; }
        }

        #endregion Properties
    }
}