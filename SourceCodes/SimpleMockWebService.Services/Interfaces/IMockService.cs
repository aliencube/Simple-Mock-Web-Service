using SimpleMockWebService.Configurations;
using System;
using System.Collections.Specialized;

namespace SimpleMockWebService.Services.Interfaces
{
    public interface IMockService : IDisposable
    {
        #region Methods

        /// <summary>
        /// Gets the mocking response from the preset value.
        /// </summary>
        /// <param name="nvc">NameValueCollection instance that represents querystrings.</param>
        /// <returns>Returns the mocking response from the preset value.</returns>
        string GetResponse(NameValueCollection nvc);

        /// <summary>
        /// Checks whether the given URL starts with the valid prefix or not.
        /// </summary>
        /// <param name="url">URL to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given URL starts with the valid prefix; otherwise returns <c>False</c>.</returns>
        bool IsValidPrefix(string url);

        /// <summary>
        /// Gets the Web API controller name from the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API controller name.</param>
        /// <returns>Returns the Web API controller name.</returns>
        string GetApiController(string url);

        /// <summary>
        /// Gets the Web API Group element based on the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API Group element.</param>
        /// <returns>Returns the Web API Group element.</returns>
        ApiGroupElement GetApiGroupElement(string url);

        /// <summary>
        /// Gets the Web API element based on the URL provided.
        /// </summary>
        /// <param name="url">URL to get the Web API element.</param>
        /// <returns>Returns the Web API element.</returns>
        ApiElement GetApiElement(string url);

        #endregion Methods
    }
}