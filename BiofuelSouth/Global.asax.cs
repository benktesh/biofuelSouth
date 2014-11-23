using System.Data.Entity;
using System.Data.Entity.Migrations;
using BiofuelSouth.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BiofuelSouth
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer<DbContext>(null);

        
        }
    }
}
