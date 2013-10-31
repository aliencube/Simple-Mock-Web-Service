using SimpleMockWebService.Configurations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace SimpleMockWebService.Services.Interfaces
{
    public interface IMockService : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the regular expression instance to check Web API prefix.
        /// </summary>
        Regex RegexPrefix { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the value that specifies whether the request method verb requires body content - POST or PUT - or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request method verb requires body content - POST or PUT; otherwise returns <c>False</c>.</returns>
        bool IsRequestBodyRequired(HttpRequestMessage request);

        /// <summary>
        /// Checks whether the given method verb is valid or not.
        /// </summary>
        /// <param name="method">Method verb to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given method verb is valid; otherwise returns <c>False</c>.</returns>
        bool IsValidMethod(string method);

        /// <summary>
        /// Checks whether the given URL starts with the valid prefix or not.
        /// </summary>
        /// <param name="url">URL to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given URL starts with the valid prefix; otherwise returns <c>False</c>.</returns>
        bool IsValidPrefix(string url);

        /// <summary>
        /// Checks whether the given file path has a valid JSON file extension or not.
        /// </summary>
        /// <param name="src">File path to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given file path has a valid JSON file extension; otherwise returns <c>False</c>.</returns>
        bool HasValidJsonFileExtension(string src);

        /// <summary>
        /// Gets the Web API element based on the URL provided.
        /// </summary>
        /// <param name="method">Method verb to get the Web API element.</param>
        /// <param name="url">URL to get the Web API element.</param>
        /// <returns>Returns the Web API element.</returns>
        ApiElement GetApiElement(string method, string url);

        /// <summary>
        /// Gets the list of segments from the URL, delimited with <c>/</c>.
        /// </summary>
        /// <param name="url">URL to get segments.</param>
        /// <returns>Returns the list of URL segments.</returns>
        IList<string> GetApiUrlSegments(string url);

        /// <summary>
        /// Gets the Web API controller name from the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API controller name.</param>
        /// <returns>Returns the Web API controller name.</returns>
        string GetApiController(string url);

        /// <summary>
        /// Gets the list of parameters from the URL provided.
        /// </summary>
        /// <param name="url">URL to get parameters.</param>
        /// <returns>Returns the list of parameters.</returns>
        IList<string> GetApiParameters(string url);

        /// <summary>
        /// Gets the default response file path.
        /// </summary>
        /// <param name="api">ApiElement instance.</param>
        /// <returns>Returns the default response file path.</returns>
        string GetDefaultApiResponseFilePath(ApiElement api);

        /// <summary>
        /// Gets the default response file path.
        /// </summary>
        /// <param name="method">Method verb to get the response.</param>
        /// <param name="url">URL to get the response.</param>
        /// <returns>Returns the default response file path.</returns>
        string GetDefaultApiResponseFilePath(string method, string url);

        /// <summary>
        /// Gets the full qualified response file path.
        /// </summary>
        /// <param name="method">Method verb to get the response file path.</param>
        /// <param name="url">URL to get the response file path.</param>
        /// <returns>Returns the full qualified response file path.</returns>
        string GetApiReponseFullPath(string method, string url);

        /// <summary>
        /// Gets the full qualified response file path.
        /// </summary>
        /// <param name="api">ApiElement instance.</param>
        /// <returns>Returns the full qualified response file path.</returns>
        string GetApiReponseFullPath(ApiElement api);

        /// <summary>
        /// Gets the API response string as JSON format.
        /// </summary>
        /// <param name="src">Full qualified file path.</param>
        /// <returns>Returns the API response string as JSON format.</returns>
        string GetApiResponse(string src);

        /// <summary>
        /// Gets the HTTP response to return.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the HTTP response.</returns>
        HttpResponseMessage GetHttpResponse(HttpRequestMessage request, string value = null);

        #endregion Methods
    }
}