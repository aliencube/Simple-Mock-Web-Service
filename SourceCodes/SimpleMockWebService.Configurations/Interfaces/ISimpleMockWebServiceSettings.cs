namespace SimpleMockWebService.Configurations.Interfaces
{
    /// <summary>
    /// This provides interfaces to the SimpleMockWebServiceSettings class.
    /// </summary>
    public interface ISimpleMockWebServiceSettings
    {
        /// <summary>
        /// Gets or sets the collection of API elements.
        /// </summary>
        ApiCollection Apis { get; set; }
    }
}