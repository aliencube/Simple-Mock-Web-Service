using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services.Interfaces;
using System;
using System.Collections.Specialized;
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
        /// Gets the Web API controller name from the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API controller name.</param>
        /// <returns>Returns the Web API controller name.</returns>
        public string GetApiController(string url)
        {
            var controller = url.Replace("~/", "")
                                .Replace(this._settings.GlobalSettings.WebApiPrefix + "/", "")
                                .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0];
            return controller;
        }

        /// <summary>
        /// Gets the Web API Group element based on the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API Group element.</param>
        /// <returns>Returns the Web API Group element.</returns>
        public ApiGroupElement GetApiGroupElement(string url)
        {
            var controller = this.GetApiController(url);

            var group = this._settings
                            .ApiGroups.Cast<ApiGroupElement>()
                            .SingleOrDefault(p => p.Key.ToLower() == controller.ToLower());
            return group;
        }

        /// <summary>
        /// Gets the Web API element based on the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API element.</param>
        /// <returns>Returns the Web API element.</returns>
        public ApiElement GetApiElement(string url)
        {
            var group = this.GetApiGroupElement(url);
            if (group == null)
                return null;

            var api = group.Apis.Cast<ApiElement>()
                           .SingleOrDefault(p => p.Url.ToLower() == url.ToLower());
            return api;
        }

        /// <summary>
        /// Gets the mocking response from the preset value.
        /// </summary>
        /// <param name="nvc">NameValueCollection instance that represents querystrings.</param>
        /// <returns>Returns the mocking response from the preset value.</returns>
        public string GetResponse(NameValueCollection nvc)
        {
            if (!nvc.AllKeys.Contains("url"))
                return null;

            var url = nvc["url"];
            if (!this.IsValidPrefix(url))
                return null;

            var api = this.GetApiElement(url);

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