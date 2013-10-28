using SimpleMockWebService.Configurations.Interfaces;
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
        public ServiceController(ISimpleMockWebServiceSettings settings)
        {
            this._settings = settings;
        }

        #endregion Constructors

        #region Properties

        private readonly ISimpleMockWebServiceSettings _settings;

        #endregion Properties

        #region Methods

        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = null;
            return response;
        }

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        #endregion Methods
    }
}