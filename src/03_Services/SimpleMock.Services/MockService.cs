using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using Aliencube.AppUtilities;
using Aliencube.SimpleMock.Configs;
using Aliencube.SimpleMock.Configs.Interfaces;
using Aliencube.SimpleMock.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace Aliencube.SimpleMock.Services
{
    /// <summary>
    /// This represents the mock service entity.
    /// </summary>
    public class MockService : IMockService
    {
        private readonly ISimpleMockSettings _settings;

        private bool _disposed;
        private Regex _regexPrefix;

        /// <summary>
        /// Initialises a new instance of the MockService class.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        public MockService(ISimpleMockSettings settings)
        {
            this._settings = settings;
        }

        /// <summary>
        /// Gets the regular expression instance to check Web API prefix.
        /// </summary>
        public Regex RegexPrefix
        {
            get
            {
                if (this._regexPrefix == null)
                {
                    this._regexPrefix = new Regex("^~?/(.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
                return this._regexPrefix;
            }
        }

        /// <summary>
        /// Gets the value that specifies whether the request method verb requires body content - POST or PUT - or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request method verb requires body content - POST or PUT; otherwise returns <c>False</c>.</returns>
        public bool IsRequestBodyRequired(HttpRequestMessage request)
        {
            var method = request.Method;
            var isRequestBodyRequired = method == HttpMethod.Post || method == HttpMethod.Put;
            return isRequestBodyRequired;
        }

        /// <summary>
        /// Checks whether the given method verb is valid or not.
        /// </summary>
        /// <param name="method">Method verb to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given method verb is valid; otherwise returns <c>False</c>.</returns>
        public bool IsValidMethod(string method)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                return false;
            }

            var validated = this._settings
                                .GlobalSettings
                                .Verbs
                                .Select(p => p.ToLower())
                                .Contains(method.ToLower());
            return validated;
        }

        /// <summary>
        /// Checks whether the given URL starts with the valid prefix or not.
        /// </summary>
        /// <param name="url">URL to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given URL starts with the valid prefix; otherwise returns <c>False</c>.</returns>
        public bool IsValidPrefix(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            var replacedUrl = this.RegexPrefix.Replace(url, "$1").ToLower();
            var prefix = this._settings
                             .GlobalSettings
                             .WebApiPrefix
                             .ToLower();
            var validated = replacedUrl.StartsWith(prefix);
            return validated;
        }

        /// <summary>
        /// Checks whether the given file path has a valid JSON file extension or not.
        /// </summary>
        /// <param name="src">File path to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given file path has a valid JSON file extension; otherwise returns <c>False</c>.</returns>
        public bool HasValidJsonFileExtension(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return false;
            }

            var extension = src.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last().ToLower();
            var validated = this._settings
                                .GlobalSettings
                                .JsonFileExtensions
                                .Select(p => p.ToLower())
                                .Contains(extension);
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
                          .SingleOrDefault(p => string.Equals(p.Method, method, StringComparison.CurrentCultureIgnoreCase)
                                                && string.Equals(p.Url, url, StringComparison.CurrentCultureIgnoreCase));
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
                              .Replace(string.Format("{0}/", this._settings.GlobalSettings.WebApiPrefix), "")
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
            {
                return null;
            }

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

            var filepath = string.Format("~/Responses/{0}.{1}{2}.json",
                                         method.ToLower(),
                                         controller.ToLower(),
                                         parameters != null
                                             ? string.Format(".{0}", string.Join(".", parameters))
                                             : string.Empty);
            return filepath;
        }

        /// <summary>
        /// Gets the full qualified response file path.
        /// </summary>
        /// <param name="method">Method verb to get the response file path.</param>
        /// <param name="url">URL to get the response file path.</param>
        /// <returns>Returns the full qualified response file path.</returns>
        public string GetApiReponseFullPath(string method, string url)
        {
            var api = this.GetApiElement(method, url);
            return this.GetApiReponseFullPath(api);
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
            {
                src = this.GetDefaultApiResponseFilePath(api);
            }

            string fullpath;
            using (var appUtility = new AppUtility())
            {
                string result;
                fullpath = appUtility.TryMapPath(src, out result) ? result : appUtility.MapPath("~/");
            }

            if (File.Exists(fullpath))
            {
                return fullpath;
            }

            var assembly = Assembly.GetExecutingAssembly();
            var codebase = String.Join("/", assembly.CodeBase
                                        .Replace("file:///", "")
                                        .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                                        .TakeWhile(p => !p.ToLower().EndsWith(".dll")));

            fullpath = (codebase + src.Replace("~/", "/")).Replace("/", "\\");
            if (HostingEnvironment.IsHosted)
            {
                fullpath = fullpath.Replace(@"\bin\", @"\");
            }

            if (File.Exists(fullpath))
            {
                return fullpath;
            }

            return null;
        }

        /// <summary>
        /// Gets the API response string as JSON format.
        /// </summary>
        /// <param name="src">Full qualified file path.</param>
        /// <returns>Returns the API response string as JSON format.</returns>
        public async Task<string> GetApiResponseAsync(string src)
        {
            if (!File.Exists(src))
            {
                return null;
            }

            string response;
            using (var reader = new StreamReader(src))
            {
                response = await reader.ReadToEndAsync();
            }
            return response;
        }

        /// <summary>
        /// Gets the HTTP response to return.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the HTTP response.</returns>
        public async Task<HttpResponseMessage> GetHttpResponseAsync(HttpRequestMessage request, string value = null)
        {
            if (this.IsRequestBodyRequired(request) && string.IsNullOrWhiteSpace(value))
            {
                return request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var method = request.Method.Method;
            if (!this.IsValidMethod(method))
            {
                return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }

            var url = request.GetQueryNameValuePairs().ToDictionary(p => p.Key, p => p.Value)["url"];
            if (!this.IsValidPrefix(url))
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            var src = this.GetApiReponseFullPath(method, url);
            if (string.IsNullOrWhiteSpace(src))
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            var json = await this.GetApiResponseAsync(src);
            var response = string.IsNullOrWhiteSpace(json)
                               ? request.CreateResponse(HttpStatusCode.NoContent)
                               : request.CreateResponse(HttpStatusCode.OK, json.StartsWith("[")
                                                                               ? JArray.Parse(json)
                                                                               : JToken.Parse(json));
            return response;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            this._disposed = true;
        }
    }
}