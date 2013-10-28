using SimpleMockWebService.Configurations;
using System;
using System.Collections.Generic;

namespace SimpleMockWebService.Services.Interfaces
{
    public interface IMockService : IDisposable
    {
        #region Methods

        /// <summary>
        /// Checks whether the given URL starts with the valid prefix or not.
        /// </summary>
        /// <param name="url">URL to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given URL starts with the valid prefix; otherwise returns <c>False</c>.</returns>
        bool IsValidPrefix(string url);

        /// <summary>
        /// Checks whether the given method verb is valid or not.
        /// </summary>
        /// <param name="method">Method verb to be validated.</param>
        /// <returns>Returns <c>True</c>, if the given method verb is valid; otherwise returns <c>False</c>.</returns>
        bool IsValidMethod(string method);

        /// <summary>
        /// Gets the Web API element based on the URL provided.
        /// </summary>
        /// <param name="method">Method verb to get the Web API element.</param>
        /// <param name="url">URL to get the Web API element.</param>
        /// <returns>Returns the Web API element.</returns>
        ApiElement GetApiElement(string method, string url);

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
        string GetResponse(IDictionary<string, string> items);

        #endregion Methods
    }
}