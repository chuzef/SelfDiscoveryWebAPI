using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Principal;
using Models.Attributes;

namespace SelfDiscoveryWebAPI.Models
{
    /// <summary>
    /// Collection of methods related to ExposedAttribute: invokation and discovery.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Invokes an ExposedAttribute method in an ExposedAttribute class.
        /// </summary>
        /// <param name="user">IPrincipal for testing method authorization</param>
        /// <param name="typeExposedName">Name of the ExposedAttribute in type</param>
        /// <param name="methodExposedName">Name of the ExposedAttribute in method</param>
        /// <param name="methodParameters">Dictionary of method parameters. Will be casted to proper types.</param>
        /// <returns>Invoked method response</returns>
        public static object Invoke(IPrincipal user, string typeExposedName, string methodExposedName, NameValueCollection methodParameters)
        {
            Type type = FindExposedType(typeExposedName);
            if (null == type)
                throw new NotImplementedException(string.Format("Type '{0}' not found in loaded assemblies or not marked as Exposed(Name='{0}').", typeExposedName));

            MethodInfo minfo = FindExposedMethod(type, methodExposedName);
            if (null == minfo)
                throw new NotImplementedException(string.Format("Method '{0}' not found in type '{1}' or not marked as Exposed(Name='{0}').", methodExposedName, typeExposedName));

#if ENABLE_AUTHORIZATION
            if (!IsAuthorized(user, minfo))
                throw new AuthenticationException();
#endif
            var parValues = (from pinfo in minfo.GetParameters()
                             let attrName = pinfo.GetExposedParameterName()
                             select string.IsNullOrEmpty(methodParameters[attrName]) ? null : Convert.ChangeType(methodParameters[attrName], pinfo.ParameterType));

            var result = minfo.Invoke(null, BindingFlags.Static, null, parValues.ToArray(), CultureInfo.CurrentCulture);

            return result;
        }

        /// <summary>
        /// From loaded assemblies fetches types and methods with ExposedAttribute and outputs
        /// invoking information.
        /// </summary>
        /// <returns>Array of ExposedMethodInfo</returns>
        public static IEnumerable<ExposedMethodInfo> DiscoverExposedMethods()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(loadedAssembly => loadedAssembly.GetTypes()).Where(
               loadedType => loadedType.GetCustomAttributes().Any(attr => attr is ExposedAttribute)).SelectMany(
               type => type.GetExposedMethodInfos());
        }

        private static bool IsAuthorized(IPrincipal user, MemberInfo minfo)
        {
            var exposed = minfo.GetCustomAttributes().OfType<ExposedAttribute>().Single();
            return exposed.Roles.Any(user.IsInRole);
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
    }
}