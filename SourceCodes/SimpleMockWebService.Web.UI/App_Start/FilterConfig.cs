using System.Web;
using System.Web.Mvc;

namespace SimpleMockWebService.Web.UI.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}