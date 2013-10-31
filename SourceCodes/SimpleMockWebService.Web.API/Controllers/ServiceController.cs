using SimpleMockWebService.Configurations;
using SimpleMockWebService.Configurations.Interfaces;
using SimpleMockWebService.Services;
using SimpleMockWebService.Services.Interfaces;
using System.Configuration;
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

        ///// <summary>
        ///// Initialises a new instance of the ServiceController class.
        ///// </summary>
        ///// <param name="settings">Configuration settings instance.</param>
        ///// <param name="service">Mock service instance.</param>
        //public ServiceController(ISimpleMockWebServiceSettings settings, IMockService service)
        //{
        //    this._settings = settings;
        //    this._service = service;
        //}

        #endregion Constructors

        #region Properties

        private ISimpleMockWebServiceSettings _settings;

        public ISimpleMockWebServiceSettings Settings
        {
            get
            {
                if (this._settings == null)
                    this._settings = ConfigurationManager.GetSection("simpleMockWebService") as SimpleMockWebServiceSettings;
                return this._settings;
            }
        }

        private IMockService _service;

        public IMockService Service
        {
            get
            {
                if (this._service == null)
                    this._service = new MockService(this.Settings);
                return this._service;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs the action for the GET method.
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Get()
        {
            var response = this._service.GetResponse(Request);
            return response;
        }

        /// <summary>
        /// Performs the action for the POST method.
        /// </summary>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Post([FromBody]string value)
        {
            var response = this._service.GetResponse(Request, value);
            return response;
        }

        /// <summary>
        /// Performs the action for the PUT method.
        /// </summary>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Put([FromBody]string value)
        {
            var response = this._service.GetResponse(Request, value);
            return response;
        }

        /// <summary>
        /// Performs the action for the DELETE method.
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Delete()
        {
            var response = this._service.GetResponse(Request);
            return response;
        }

        #endregion Methods
    }
}