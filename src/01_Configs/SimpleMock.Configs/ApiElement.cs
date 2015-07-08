using System.Configuration;
using System.Text.RegularExpressions;

namespace Aliencube.SimpleMock.Configs
{
    /// <summary>
    /// This represents the API element entity.
    /// </summary>
    public class ApiElement : ConfigurationElement
    {
        private Regex _regexSourcePath;

        /// <summary>
        /// Gets the regular expression instance to check Web API prefix.
        /// </summary>
        public Regex RegexSourcePath
        {
            get
            {
                if (this._regexSourcePath != null)
                {
                    return this._regexSourcePath;
                }

                this._regexSourcePath = new Regex("^[A-Z]+:\\.+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return this._regexSourcePath;
            }
        }

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
        /// Gets or sets the group key where the element explicitly belongs.
        /// Default value is <c>String.Empty</c>.
        /// If this is not set, it assumes that the element belongs to the parent API group.
        /// </summary>
        [ConfigurationProperty("group", DefaultValue = "", IsRequired = false)]
        public string Group
        {
            get { return (string)this["group"]; }
            set { this["group"] = value; }
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
        /// Default value is <c>String.Empty</c>.
        /// If this is not set, it assumes that the JSON result is located at <c>~/Responses</c>,
        /// with a name combining method and URL.
        /// </summary>
        [ConfigurationProperty("src", DefaultValue = "", IsRequired = false)]
        public string Src
        {
            get { return this.GetResponsePath((string)this["src"]); }
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

        /// <summary>
        /// Gets the application file path where the response is located.
        /// </summary>
        /// <param name="src">Filepath.</param>
        /// <returns>Returns the application file path where the response is located.</returns>
        /// <remarks>
        /// This has basic assumptions on the file path:
        ///     <list type="bullet">
        ///         <item>The return value is <c>String.Empty</c>, if the file path is <c>null</c> or empty.</item>
        ///         <item>The return value is the input value, if the file path is the full qualified file path. eg) <c>C:\Responses\get.contents.json</c></item>
        ///         <item>The return value is the input value, if the file path is the application file path. eg) <c>~/Responses/get.contents.json</c></item>
        ///         <item>The return value prepends the application root symbol (<c>~</c>) to the given path, if the file path starts with <c>/</c>. eg) <c>/Responses/get.contents.json</c></item>
        ///         <item>The return value prepends the default location (<c>~/Responses</c>) to the given path, if nothing applies above. eg) <c>get.contents.json</c></item>
        ///     </list>
        /// </remarks>
        private string GetResponsePath(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            if (this.RegexSourcePath.IsMatch(src))
            {
                return src;
            }

            if (src.StartsWith("~/"))
            {
                return src;
            }

            src = string.Format(src.StartsWith("/") ? "~{0}" : "~/Responses/{0}", src);
            return src;
        }
    }
}