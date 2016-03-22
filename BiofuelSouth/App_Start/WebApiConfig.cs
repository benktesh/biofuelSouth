using System.Web.Http;
using RouteParameter = System.Web.Http.RouteParameter;

namespace BiofuelSouth
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
           config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional }
            );
        }

   
    }
}
