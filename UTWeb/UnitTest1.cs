using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace UTWeb
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            BusinessModel1 bm1 = new BusinessModel1("property AAA");

            var controller = new BusinessController1 {Request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = new ObjectContent(bm1.GetType(), bm1, new JsonMediaTypeFormatter())
            }};
            controller.Request.SetConfiguration(new HttpConfiguration());

            var response = controller.Index();
            var entity = GetContent<BusinessModel3>(response.Content);
            Assert.AreNotEqual(entity, null);
        }        
        
        [TestMethod]
        public void TestMethod2()
        {
            BusinessModel1 bm1 = new BusinessModel1("property AAA");

            var controller = new BusinessController1 {Request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = new ObjectContent(bm1.GetType(), bm1, new JsonMediaTypeFormatter())
            }};
            controller.Request.SetConfiguration(new HttpConfiguration());

            var response = controller.Index();
            var entity = GetContent<BusinessModel3>(response.Content);
            Assert.AreNotEqual(entity, null);
        }

        private static T GetContent<T>(HttpContent content) where T: class
        {
            string jsonString = content.ReadAsStringAsync().Result;
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(jsonString));
            var newObject = serializer.ReadObject(memoryStream) as T;
            memoryStream.Close();
            return newObject;
        }
    }
}
