using System;

namespace Aliencube.SimpleMock.Configs.Interfaces
{
    /// <summary>
    /// This provides interfaces to the SimpleMockWebServiceSettings class.
    /// </summary>
    public interface ISimpleMockSettings : IDisposable
    {
        /// <summary>
        /// Gets or sets the global settings element.
        /// </summary>
        GlobalSettingsElement GlobalSettings { get; set; }

        /// <summary>
        /// Gets or sets the collection of API element groups.
        /// </summary>
        ApiGroupElementCollection ApiGroups { get; set; }
    }
}