using System.Web;
using System.Web.Optimization;

namespace XOVO
{
    public class BundleConfig
    {
        // Weitere Informationen zur Bündelung finden Sie unter https://go.microsoft.com/fwlink/?LinkId=301862.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/css/").Include("~/Content/css/materialize.css", "~/Content/css/login_registration.css"));

            bundles.Add(new StyleBundle("~/main/css/").Include("~/Content/css/materialize.css", "~/Content/css/homepage.css"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include("~/Content/js/materialize.js"));

            bundles.Add(new ScriptBundle("~/bundles/colorpicker/").Include("~/Content/js/bootstrap-colorpicker.js"));

        }
    }
}
