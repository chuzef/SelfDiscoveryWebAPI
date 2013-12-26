using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SelfDiscoveryWebAPI.Models;

namespace SelfDiscoveryWebAPI.Controllers
{
    public class CareConnectController : BaseApiController
    {
//        public HttpResponseMessage Index()
//        {
//            var bm1 = GetContentAs<BusinessModel1>(Request.Content);
////            var bm1 = new BusinessModel1("data1");
//            var bm2 = new BusinessModel2("data2");
//
//            var result = BusinessLogic1.GetBusinessModel3(bm1, bm2);
//            return Request.CreateResponse(HttpStatusCode.OK, result);
//        }

        /// <summary>
        /// Invokes method 'invokeMethod' of 'invokeClass'. Provide parameters for the method in query string. 
        /// Mehtods, their input parameters and output parameter are described at /careconnect/discover.
        /// </summary>
        /// <param name="invokeClass">Name property of ExposeAttribute of a class to be invoked</param>
        /// <param name="invokeMethod">Name property of ExposeAttribute of a method to be invoked</param>
        /// <returns>Depends on the invoked class. Use headers in your Http request to control response format e.g. json/xml.</returns>
        [HttpGet]
        public HttpResponseMessage Rpc(string invokeClass, string invokeMethod)
        {
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