using System.Data.Entity;
using System.Web;
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

        
        }
    }
}
