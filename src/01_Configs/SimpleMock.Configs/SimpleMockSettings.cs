using System;
using System.Configuration;
using Aliencube.SimpleMock.Configs.Interfaces;

namespace Aliencube.SimpleMock.Configs
{
    /// <summary>
    /// This represents the configuration settings entity for Simple Mock Web Service.
    /// </summary>
    public class SimpleMockSettings : ConfigurationSection, ISimpleMockSettings
    {
        private bool _disposed;

        /// <summary>
        /// Gets or sets the global settings element.
        /// </summary>
        [ConfigurationProperty("globalSettings", IsRequired = true)]
        public GlobalSettingsElement GlobalSettings
        {
            get { return (GlobalSettingsElement)this["globalSettings"]; }
            set { this["globalSettings"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of API element groups.
        /// </summary>
        [ConfigurationProperty("apiGroups", IsRequired = true)]
        public ApiGroupElementCollection ApiGroups
        {
            get { return (ApiGroupElementCollection)this["apiGroups"]; }
            set { this["apiGroups"] = value; }
        }

        /// <summary>
        /// Creates a new instance of the <c>ConverterSettings</c> class.
        /// </summary>
        /// <returns>Returns the new instance of the <c>ConverterSettings</c> class.</returns>
        public static ISimpleMockSettings CreateInstance()
        {
            var settings = GetFromSimpleMockSettings();
            if (settings == null)
            {
                throw new InvalidOperationException("Settings not found");
            }

            return settings;
        }

        /// <summary>
        /// Gets the <c>ConverterSettings</c> object from the converterSettings element.
        /// </summary>
        /// <returns>Returns the <c>ConverterSettings</c> object.</returns>
        private static ISimpleMockSettings GetFromSimpleMockSettings()
        {
            var settings = ConfigurationManager.GetSection("simpleMockSettings") as ISimpleMockSettings;
            return settings;
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