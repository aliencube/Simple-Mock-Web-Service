using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace SimpleMockWebService.Services
{
    /// <summary>
    /// This represents the mock service entity.
    /// </summary>
    public class MockService : IMockService
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MockService class.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        public MockService(ISimpleMockWebServiceSettings settings)
        {
            this._settings = settings;
        }

        #endregion Constructors

        #region Properties

        private readonly ISimpleMockWebServiceSettings _settings;

        private Regex _regexPrefix;

        /// <summary>
        /// Gets the regular expression instance to check Web API prefix.
        /// </summary>
        public Regex RegexPrefix
        {
            get
            {
                if (this._regexPrefix == null)
                    this._regexPrefix = new Regex("^~?/(.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return this._regexPrefix;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Checks whether the given URL starts with the valid prefix or not.
        /// </summary>
        /// <param name="url">URL to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given URL starts with the valid prefix; otherwise returns <c>False</c>.</returns>
        public bool IsValidPrefix(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
                return false;

            url = this.RegexPrefix.Replace(url, "$1").ToLower();
            var validated = url.StartsWith(this._settings.GlobalSettings.WebApiPrefix.ToLower());
            return validated;
        }

        /// <summary>
        /// Checks whether the given method verb is valid or not.
        /// </summary>
        /// <param name="method">Method verb to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given method verb is valid; otherwise returns <c>False</c>.</returns>
        public bool IsValidMethod(string method)
        {
            if (String.IsNullOrWhiteSpace(method))
                return false;

            var validated = this._settings
                                .GlobalSettings
                                .Verbs
                                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.ToLower())
                                .Contains(method.ToLower());
            return validated;
        }

        /// <summary>
        /// Checks whether the given file path has a valid JSON file extension or not.
        /// </summary>
        /// <param name="src">File path to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given file path has a valid JSON file extension; otherwise returns <c>False</c>.</returns>
        public bool HasValidJsonFileExtension(string src)
        {
            if (String.IsNullOrWhiteSpace(src))
                return false;

            var validated = this._settings
                                .GlobalSettings
                                .JsonFileExtensions
                                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.ToLower())
                                .Contains(src.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last());
            return validated;
        }

        /// <summary>
        /// Gets the Web API element based on the URL provided.
        /// </summary>
        /// <param name="method">Method verb to get the Web API element.</param>
        /// <param name="url">URL to get the Web API element.</param>
        /// <returns>Returns the Web API element.</returns>
        public ApiElement GetApiElement(string method, string url)
        {
            var api = this._settings
                          .ApiGroups
                          .Cast<ApiGroupElement>()
                          .SelectMany(p => p.Apis.Cast<ApiElement>())
                          .SingleOrDefault(p => p.Method.ToLower() == method.ToLower()
                                                && p.Url.ToLower() == url.ToLower());
            return api;
        }

        /// <summary>
        /// Gets the list of segments from the URL, delimited with <c>/</c>.
        /// </summary>
        /// <param name="url">URL to get segments.</param>
        /// <returns>Returns the list of URL segments.</returns>
        public IList<string> GetApiUrlSegments(string url)
        {
            var segments = url.Replace("~/", "")
                              .Replace(this._settings.GlobalSettings.WebApiPrefix + "/", "")
                              .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return segments;
        }

        /// <summary>
        /// Gets the Web API controller name from the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API controller name.</param>
        /// <returns>Returns the Web API controller name.</returns>
        public string GetApiController(string url)
        {
            var controller = this.GetApiUrlSegments(url)[0];
            return controller;
        }

        /// <summary>
        /// Gets the list of parameters from the URL provided.
        /// </summary>
        /// <param name="url">URL to get parameters.</param>
        /// <returns>Returns the list of parameters.</returns>
        public IList<string> GetApiParameters(string url)
        {
            var segments = this.GetApiUrlSegments(url);
            if (segments.Count == 1)
                return null;

            var parameters = segments.Skip(1).ToList();
            return parameters;
        }

        /// <summary>
        /// Gets the default response file path.
        /// </summary>
        /// <param name="api">ApiElement instance.</param>
        /// <returns>Returns the default response file path.</returns>
        public string GetDefaultApiResponseFilePath(ApiElement api)
        {
            return this.GetDefaultApiResponseFilePath(api.Method, api.Url);
        }

        /// <summary>
        /// Gets the default response file path.
        /// </summary>
        /// <param name="method">Method verb to get the response.</param>
        /// <param name="url">URL to get the response.</param>
        /// <returns>Returns the default response file path.</returns>
        public string GetDefaultApiResponseFilePath(string method, string url)
        {
            var controller = this.GetApiController(url);
            var parameters = this.GetApiParameters(url);

            var filepath = String.Format("~/Responses/{0}.{1}{2}.json",
                                         method.ToLower(),
                                         controller.ToLower(),
                                         parameters != null
                                             ? String.Format(".{0}", String.Join(".", parameters))
                                             : String.Empty);
            return filepath;
        }

        /// <summary>
        /// Gets the full qualified response file path.
        /// </summary>
        /// <param name="api">ApiElement instance.</param>
        /// <returns>Returns the full qualified response file path.</returns>
        public string GetApiReponseFullPath(ApiElement api)
        {
            var src = api.Src;
            if (!this.HasValidJsonFileExtension(src))
                src = this.GetDefaultApiResponseFilePath(api);

            string fullpath;
            if (HostingEnvironment.IsHosted)
            {
                fullpath = HostingEnvironment.MapPath(src);
                return fullpath;
            }

            var assembly = Assembly.GetExecutingAssembly();
            var directory = Path.GetDirectoryName(assembly.Location);
            fullpath = (directory + src.Replace("~/", "/")).Replace("/", "\\");
            if (File.Exists(fullpath))
                return fullpath;

            var codebase = String.Join("/", assembly.CodeBase
                                                    .Replace("file:///", "")
                                                    .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                                                    .TakeWhile(p => !p.ToLower().EndsWith(".dll")));
            fullpath = (codebase + src.Replace("~/", "/")).Replace("/", "\\");
            if (File.Exists(fullpath))
                return fullpath;

            return null;
        }

        /// <summary>
        /// Gets the API response string as JSON format.
        /// </summary>
        /// <param name="src">Full qualified file path.</param>
        /// <returns>Returns the API response string as JSON format.</returns>
        public string GetApiResponse(string src)
        {
            if (!File.Exists(src))
                return null;

            string response;
            using (var reader = new StreamReader(src))
            {
                response = reader.ReadToEnd();
            }
            return response;
        }

        /// <summary>
        /// Gets the mocking response from the preset value.
        /// </summary>
        /// <param name="items">List of items to fetch response.</param>
        /// <returns>
        /// Returns either:
        ///     <list type="bullet">
        ///         <item>The mocking response from the preset value, or</item>
        ///         <item>
        ///             <c>null</c>, if the input items don't have URL, method verb or source file path,
        ///             or invalid URL, method verb or source file path.</item>
        ///     </list>
        /// </returns>
        public string GetResponse(IDictionary<string, string> items)
        {
            if (!items.ContainsKey("method"))
                return null;

            if (!items.ContainsKey("url"))
                return null;

            if (!items.ContainsKey("src"))
                return null;

            var method = items["method"];
            if (!this.IsValidMethod(method))
                return null;

            var url = items["url"];
            if (!this.IsValidPrefix(url))
                return null;

            var src = items["src"];
            if (String.IsNullOrWhiteSpace(src))
                return null;

            var response = this.GetApiResponse(src);
            return response;
        }

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