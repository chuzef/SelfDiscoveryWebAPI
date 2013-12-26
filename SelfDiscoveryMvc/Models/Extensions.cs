using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Models.Attributes;

namespace SelfDiscoveryMvc.Models
{
    public static class Extensions
    {
        public static IEnumerable<ExposedMethodInfo> GetExposedMethodInfos(this Type type)
        {
            var result = new List<ExposedMethodInfo>();
            var typeExposedAttr = type.GetCustomAttributes(true).OfType<ExposedAttribute>().Single();

            foreach (var minfo in type.GetMethods().Where(mm => mm.GetCustomAttributes(true).OfType<ExposedAttribute>().Any()))
            {
                var methodExposedAttr = minfo.GetCustomAttributes(true).OfType<ExposedAttribute>().Single();
                var parinfo = minfo.GetParameters().Select(pp => string.Format("{0}=", pp.GetExposedParameterName()));
                result.Add(new ExposedMethodInfo
                {
                    UrlTemplate = string.Format("http://<host>/api/{0}/{1}?{2}",
                        typeExposedAttr.Name, methodExposedAttr.Name, string.Join("&", parinfo)),
                    ActionName = methodExposedAttr.Name,
                    ControllerName = type.GetExposedTypeName(),
                    Parameters = minfo.GetParameters().Select(pp => new ExposedMethodInfo.ExposedParameterInfo
                    {
                        Name = pp.GetExposedParameterName(), 
                        Type = pp.ParameterType.ToString(),
                        Description = pp.GetExposedParameterDescription(),
                    }).ToArray(),
                    Description = methodExposedAttr.Description,
                    ReturnType = minfo.ReturnType.ToString()
                });
            }
            return result;
        }

        public static string GetExposedTypeName(this Type tinfo)
        {
            return tinfo.GetCustomAttributes(typeof(ExposedAttribute), true).Any()
                ? tinfo.GetCustomAttributes(typeof(ExposedAttribute), true).OfType<ExposedAttribute>().Single().Name
                : tinfo.Name;
        }

        public static string GetExposedParameterName(this ParameterInfo pinfo)
        {
            return pinfo.GetCustomAttributes(typeof(ExposedAttribute), true).Any()
                ? pinfo.GetCustomAttributes(typeof(ExposedAttribute), true).OfType<ExposedAttribute>().Single().Name
                : pinfo.Name;
        }

        public static string GetExposedParameterDescription(this ParameterInfo pinfo)
        {
            return pinfo.GetCustomAttributes(typeof(ExposedAttribute), true).Any()
                ? pinfo.GetCustomAttributes(typeof(ExposedAttribute), true).OfType<ExposedAttribute>().Single().Description
                : string.Empty;
        }
    }
}