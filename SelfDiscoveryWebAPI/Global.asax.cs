using System;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SelfDiscoveryWebAPI.Infrastructure;

namespace SelfDiscoveryWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthenticationTokenDelegatingHandler());

            // remove XML formater, as it cannot serialize dynamic classes. will respond json only
            var configuration = GlobalConfiguration.Configuration;
            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);

            LoadBllAssemblies();
        }

        private static void LoadBllAssemblies()
        {
            var bllAssembliesUnderBinDirectory = WebConfigurationManager.AppSettings["BLL_ASSEMBLIES_PATH_UNDER_BIN"];
            string bllAssembliesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin",  bllAssembliesUnderBinDirectory);

            DirectoryInfo directory = new DirectoryInfo(bllAssembliesPath);
            FileInfo[] files = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {
                // Load the file into the application domain.
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(file.FullName);
                Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
            }
        }
    }
}
