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
            var validated = this._settings
                                .GlobalSettings
                                .Verbs
                                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.ToLower())
                                .Contains(method.ToLower());
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
        /// Gets the mocking response from the preset value.
        /// </summary>
        /// <param name="items">List of items to fetch response.</param>
        /// <returns>
        /// Returns either:
        ///     <list type="bullet">
        ///         <item>The mocking response from the preset value, or</item>
        ///         <item><c>null</c>, if the input items don't have URL or method verb, or invalid URL or method verb.</item>
        ///     </list>
        /// </returns>
        public string GetResponse(IDictionary<string, string> items)
        {
            if (!items.ContainsKey("url"))
                return null;

            if (!items.ContainsKey("method"))
                return null;

            var url = items["url"];
            if (!this.IsValidPrefix(url))
                return null;

            var method = items["method"];
            if (!this.IsValidMethod(method))
                return null;

            var api = this.GetApiElement(method, url);

            if (api == null)
                return null;

            var src = api.Src;
            var response = String.Empty;
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