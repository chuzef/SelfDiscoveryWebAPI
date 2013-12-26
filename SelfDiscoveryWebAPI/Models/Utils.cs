using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Models.Attributes;

namespace SelfDiscoveryWebAPI.Models
{
    public static class Utils
    {
        public static object Invoke(string typeExposedName, string methodExposedName, NameValueCollection methodParameters)
        {
            Type type = FindExposedType(typeExposedName);
            if (null == type)
                throw new NotImplementedException(string.Format("Type '{0}' not found in loaded assemblies or not marked as Exposed(Name='{0}').", typeExposedName));

            MethodInfo minfo = FindExposedMethod(type, methodExposedName);
            if (null == minfo)
                throw new NotImplementedException(string.Format("Method '{0}' not found in type '{1}' or not marked as Exposed(Name='{0}').", methodExposedName, typeExposedName));

            var parValues = (from pinfo in minfo.GetParameters()
                             let attrName = pinfo.GetExposedParameterName()
                             select null == methodParameters[attrName] ? null : Convert.ChangeType(methodParameters[attrName], pinfo.ParameterType));

            var result = minfo.Invoke(null, BindingFlags.Static, null, parValues.ToArray(), CultureInfo.CurrentCulture);

            return result;
        }

        private static Type FindExposedType(string exposedTypeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(loadedAssembly => loadedAssembly.GetTypes()).FirstOrDefault(
                loadedType => loadedType.GetCustomAttributes().Any(attr => attr is ExposedAttribute &&
                    (attr as ExposedAttribute).Name == exposedTypeName));
        }

        private static MethodInfo FindExposedMethod(Type type, string exposedMethodName)
        {
            return type.GetMethods().FirstOrDefault(method =>
                method.GetCustomAttributes().Any(attr => attr is ExposedAttribute &&
                    (attr as ExposedAttribute).Name == exposedMethodName));
        }

        public static IEnumerable<ExposedMethodInfo> DiscoverExposedMethods()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(loadedAssembly => loadedAssembly.GetTypes()).Where(
               loadedType => loadedType.GetCustomAttributes().Any(attr => attr is ExposedAttribute)).SelectMany(
               type => type.GetExposedMethodInfos());
        }
    }
}