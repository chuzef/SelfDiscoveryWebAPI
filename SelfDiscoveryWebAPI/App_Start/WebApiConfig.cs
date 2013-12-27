using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;
using Models;

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

//            RegisterODataEndpoint(config);
        }

        private static void RegisterODataEndpoint(HttpConfiguration config)
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();

            // Defining EDM model        
            modelBuilder.EntitySet<BusinessModel2>("Posts");
            IEdmModel model = modelBuilder.GetEdmModel();

            config.Routes.MapODataRoute(routeName: "Posts", routePrefix: "odata", model: model);

            config.Routes.MapHttpBatchRoute(routeName: "batch",
                routeTemplate: "api/batch", 
                batchHandler: new DefaultHttpBatchHandler(GlobalConfiguration.DefaultServer));
        }
    }
}
