using SimpleMockWebService.Services.Interfaces;
using System.Net.Http;
using System.Web.Http;

namespace SimpleMockWebService.Web.API.Controllers
{
    /// <summary>
    /// This represents the Web API service controller entity.
    /// </summary>
    public class ServiceController : ApiController
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ServiceController class.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        /// <param name="service">Mock service instance.</param>
        public ServiceController(IConfigurationSettings settings, IMockService service)
        {
            this._settings = settings;
            this._service = service;
        }

        #endregion Constructors

        #region Properties

        private readonly IConfigurationSettings _settings;
        private readonly IMockService _service;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs the action for the GET method.
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Get()
        {
            var response = this._service.GetHttpResponse(Request);
            return response;
        }

        /// <summary>
        /// Performs the action for the POST method.
        /// </summary>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Post([FromBody]string value)
        {
            var response = this._service.GetHttpResponse(Request, value);
            return response;
        }

        /// <summary>
        /// Performs the action for the PUT method.
        /// </summary>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Put([FromBody]string value)
        {
            var response = this._service.GetHttpResponse(Request, value);
            return response;
        }

        /// <summary>
        /// Performs the action for the DELETE method.
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Delete()
        {
            var response = this._service.GetHttpResponse(Request);
            return response;
        }

        #endregion Methods
    }
}