using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Aliencube.SimpleMock.Configs.Interfaces;
using Aliencube.SimpleMock.Services.Interfaces;

namespace Aliencube.SimpleMock.WebApi.Controllers
{
    /// <summary>
    /// This represents the Web API service controller entity.
    /// </summary>
    public class ServiceController : ApiController
    {
        private readonly ISimpleMockSettings _settings;
        private readonly IMockService _service;

        /// <summary>
        /// Initialises a new instance of the ServiceController class.
        /// </summary>
        /// <param name="settings">Configuration settings instance.</param>
        /// <param name="service">Mock service instance.</param>
        public ServiceController(ISimpleMockSettings settings, IMockService service)
        {
            this._settings = settings;
            this._service = service;
        }

        /// <summary>
        /// Performs the action for the GET method.
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public async Task<HttpResponseMessage> Get()
        {
            var response = await this._service.GetHttpResponseAsync(Request);
            return response;
        }

        /// <summary>
        /// Performs the action for the POST method.
        /// </summary>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public async Task<HttpResponseMessage> Post([FromBody]string value)
        {
            var response = await this._service.GetHttpResponseAsync(Request);
            return response;
        }

        /// <summary>
        /// Performs the action for the PUT method.
        /// </summary>
        /// <param name="value">JSON string from the request body.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public async Task<HttpResponseMessage> Put([FromBody]string value)
        {
            var response = await this._service.GetHttpResponseAsync(Request);
            return response;
        }

        /// <summary>
        /// Performs the action for the DELETE method.
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public async Task<HttpResponseMessage> Delete()
        {
            var response = await this._service.GetHttpResponseAsync(Request);
            return response;
        }
    }
}