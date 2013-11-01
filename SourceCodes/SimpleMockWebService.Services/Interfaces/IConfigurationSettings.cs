using SimpleMockWebService.Configurations.Interfaces;
using System;

namespace SimpleMockWebService.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the ConfigurationSettings class.
    /// </summary>
    public interface IConfigurationSettings : IDisposable
    {
        /// <summary>
        /// Gets the SimpleMockWebService element section.
        /// </summary>
        ISimpleMockWebServiceSettings SimpleMockWebServiceSettings { get; }
    }
}