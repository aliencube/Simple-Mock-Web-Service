using System.IO;
using System.Text;
using System.Web.Hosting;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var validated = url.Replace("~/", "").ToLower()
                               .StartsWith(this._settings.GlobalSettings.WebApiPrefix.ToLower());
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
                                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.ToLower())
                                .Contains(src.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries).Last());
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

        public IList<string> GetApiUrlSegments(string url)
        {
            var segments = url.Replace("~/", "")
                              .Replace(this._settings.GlobalSettings.WebApiPrefix + "/", "")
                              .Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
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

        public IList<string> GetApiParameters(string url)
        {
            var segments = this.GetApiUrlSegments(url);
            if (segments.Count == 1)
                return null;

            var parameters = segments.Skip(1).ToList();
            return parameters;
        }

        public string GetDefaultApiResponseFilePath(string method, string url)
        {
            var controller = this.GetApiController(url);
            var parameters = this.GetApiParameters(url);

            var filepath = String.Format("~/responses/{0}.{1}.{2}.json",
                                         method.ToLower(),
                                         controller.ToLower(),
                                         String.Join(".", parameters));
            return filepath;
        }

        public string GetApiResponse(string src)
        {
            var fullpath = HostingEnvironment.MapPath(src);
            if (String.IsNullOrWhiteSpace(fullpath) || !File.Exists(fullpath))
                return null;

            string response;
            using (var reader = new StreamReader(fullpath))
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

            //var api = this.GetApiElement(method, url);
            //if (api == null)
            //    return null;
            //if (!this.HasValidJsonFileExtension(src))
            //    src = this.GetDefaultApiResponseFilePath(method, url);

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