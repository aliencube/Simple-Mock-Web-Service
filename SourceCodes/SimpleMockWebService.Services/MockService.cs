﻿using Newtonsoft.Json.Linq;
using SimpleMockWebService.Configurations;
using SimpleMockWebService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public MockService(IConfigurationSettings settings)
        {
            this._settings = settings;
        }

        #endregion Constructors

        #region Properties

        private readonly IConfigurationSettings _settings;

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
            if (String.IsNullOrWhiteSpace(method))
                return false;

            var validated = this._settings
                                .SimpleMockWebServiceSettings
                                .GlobalSettings
                                .Verbs
                                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
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
            if (String.IsNullOrWhiteSpace(url))
                return false;

            url = this.RegexPrefix.Replace(url, "$1").ToLower();
            var validated = url.StartsWith(this._settings
                                               .SimpleMockWebServiceSettings
                                               .GlobalSettings
                                               .WebApiPrefix
                                               .ToLower());
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
                                .SimpleMockWebServiceSettings
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
                          .SimpleMockWebServiceSettings
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
                              .Replace(String.Format("{0}/",
                                                     this._settings
                                                         .SimpleMockWebServiceSettings
                                                         .GlobalSettings
                                                         .WebApiPrefix), "")
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
        /// Gets the HTTP response to return.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the HTTP response.</returns>
        public HttpResponseMessage GetHttpResponse(HttpRequestMessage request, string value = null)
        {
            if (this.IsRequestBodyRequired(request) && String.IsNullOrWhiteSpace(value))
                return request.CreateResponse(HttpStatusCode.UnsupportedMediaType);

            var method = request.Method.Method;
            if (!this.IsValidMethod(method))
                return request.CreateResponse(HttpStatusCode.MethodNotAllowed);

            var query = request.GetQueryNameValuePairs();
            string url;

            if (query.Any())
            {
                url = query.ToDictionary(p => p.Key, p => p.Value)["url"];
            }
            else
            {
                url = request.RequestUri.PathAndQuery;
            }

            if (!this.IsValidPrefix(url))
                return request.CreateResponse(HttpStatusCode.NotFound);

            var src = this.GetApiReponseFullPath(method, url);
            if (String.IsNullOrWhiteSpace(src))
                return request.CreateResponse(HttpStatusCode.NotFound);

            var json = this.GetApiResponse(src);
            var response = String.IsNullOrWhiteSpace(json)
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
        }

        #endregion Methods
    }
}