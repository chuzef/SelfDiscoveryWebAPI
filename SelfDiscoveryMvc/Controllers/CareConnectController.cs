using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SelfDiscoveryMvc.Models;

namespace SelfDiscoveryMvc.Controllers
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

        [HttpGet]
        public HttpResponseMessage Rpc(string invokeClass, string invokeMethod)
        {
            var methodResult = Utils.Invoke(invokeClass, invokeMethod, Request.RequestUri.ParseQueryString());
            return Request.CreateResponse(HttpStatusCode.OK, methodResult);
        }

        [HttpGet]
        public ExposedMethodInfo[] Discover()
        {
            var methods = Utils.DiscoverExposedMethods();
            return methods.ToArray();
        }
	}
}