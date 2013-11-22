using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SimpleMockWebService.Web.UI.App_Start.BundleConfig), "RegisterBundles")]

namespace SimpleMockWebService.Web.UI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles()
        {
            //  CSS bundles.
            BundleTable.Bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/main.css",
                                                                             "~/Content/style.css"));
            //  Javascript bundles.
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-{version}.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/angularjs").Include("~/Scripts/angularjs/angular.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery/jquery-{version}.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/plugins.js",
                                                                             "~/Scripts/services.js",
                                                                             "~/Scripts/controllers.js",
                                                                             "~/Scripts/main.js"));
        }
    }
}
