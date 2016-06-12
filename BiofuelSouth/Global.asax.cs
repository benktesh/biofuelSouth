using System.Data.Entity;
using System.Web;
using System.Web.Helpers;
using System.Web.Optimization;
using System.Web.Routing;

namespace BiofuelSouth
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer<DbContext>(null);

            //PdfSharp.Fonts.GlobalFontSettings.FontResolver = new MyFontResolver();

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

        }
    }
}
