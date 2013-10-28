using System;

namespace SimpleMockWebService.Configurations.Interfaces
{
    /// <summary>
    /// This provides interfaces to the SimpleMockWebServiceSettings class.
    /// </summary>
    public interface ISimpleMockWebServiceSettings : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the global settings element.
        /// </summary>
        GlobalSettingsElement GlobalSettings { get; set; }

        /// <summary>
        /// Gets or sets the collection of API element groups.
        /// </summary>
        ApiGroupElementCollection ApiGroups { get; set; }

        #endregion Properties
    }
}