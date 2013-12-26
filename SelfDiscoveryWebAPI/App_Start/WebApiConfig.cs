using System.Web.Http;

namespace SelfDiscoveryWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // http://localhost:65201/api/careconnect/discover
            // http://localhost:65201/api
            config.Routes.MapHttpRoute(
                name: "CareConnectDiscover",
                routeTemplate: "api/careconnect/discover",
                defaults: new { controller = "careconnect", action = "discover", id = RouteParameter.Optional }
            );

            // http://localhost:65201/api/patients/encounters?patientID=aaa
            config.Routes.MapHttpRoute(
                name: "RpcApiController",
                routeTemplate: "api/{invokeClass}/{invokeMethod}",
                defaults: new { controller = "CareConnect", action = "Rpc" }
            );
        }
    }
}
