using SimpleMockWebService.Services.Interfaces;
using System.Net.Http;
using System.Web.Http;

namespace SimpleMockWebService.Web.API.SelfHost.Controllers
{
    /// <summary>
    /// This represents the Web API service controller entity.
    /// </summary>
    public class ServiceController : SimpleMockWebService.Web.API.Controllers.ServiceController
    {
        public ServiceController(IConfigurationSettings settings, IMockService service) : base(settings, service) { }
    }
}