using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models;
using SelfDiscoveryWebAPI.Models;

namespace SelfDiscoveryWebAPI.Controllers
{
    /// <summary>
    /// RPC to business layer
    /// </summary>
    public class CareConnectController : BaseApiController
    {
        /// <summary>
        /// Invokes method 'invokeMethod' of 'invokeClass'. Provide single input parameter for the method in form body. 
        /// Mehtods, their input parameters and output parameter are described at /careconnect/discover.
        /// </summary>
        /// <param name="invokeClass">Name property of ExposeAttribute of a class to be invoked</param>
        /// <param name="invokeMethod">Name property of ExposeAttribute of a method to be invoked</param>
        /// <returns>Depends on the invoked class. Use headers in your Http request to control response format e.g. json/xml.</returns>
        [HttpPost]
        [ActionName("Rpc")]
        public HttpResponseMessage RpcFromBody(string invokeClass, string invokeMethod)
        {
            var bm = new BusinessModel3(111);

            var methodResult = Utils.Invoke(User, invokeClass, invokeMethod, Request.Content);
            return Request.CreateResponse(HttpStatusCode.OK, methodResult);
        }

        /// <summary>
        /// Invokes method 'invokeMethod' of 'invokeClass'. Provide parameters for the method in query string. 
        /// Mehtods, their input parameters and output parameter are described at /careconnect/discover.
        /// </summary>
        /// <param name="invokeClass">Name property of ExposeAttribute of a class to be invoked</param>
        /// <param name="invokeMethod">Name property of ExposeAttribute of a method to be invoked</param>
        /// <returns>Depends on the invoked class. Use headers in your Http request to control response format e.g. json/xml.</returns>
        [HttpGet]
        [ActionName("Rpc")]
        public HttpResponseMessage RpcFromQueryString(string invokeClass, string invokeMethod)
        {
            var bm = new BusinessModel3(111);

            var methodResult = Utils.Invoke(User, invokeClass, invokeMethod, Request.RequestUri.ParseQueryString());
            return Request.CreateResponse(HttpStatusCode.OK, methodResult);
        }

        /// <summary>
        /// Outputs information about classes and methods in loaded assemblies that have ExposeAttribute.
        /// </summary>
        /// <returns>List of classes / methods with their parameters.</returns>
        [HttpGet]
        public ExposedMethodInfo[] Discover()
        {
            var methods = Utils.DiscoverExposedMethods();
            return methods.ToArray();
        }
	}
}