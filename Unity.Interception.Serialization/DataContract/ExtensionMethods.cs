// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Create a TypeInterceptor which allow create datacontract types.
        /// </summary>
        /// <param name="typeInterceptor">A <see cref="ITypeInterceptor"/>.</param>
        /// <returns>A <see cref="ITypeInterceptor"/> which allow create datacontract types.</returns>
        public static ITypeInterceptor AddDataContractSupport(this ITypeInterceptor typeInterceptor)
        {
            return Intercept.ThroughProxy(typeInterceptor, new InterfaceInterceptor(), new[] { new VirtualMethodInterceptorBehavior() });
        }

        internal static DataContractAttribute GetDataContractConfig(this Type dataContractType)
        {
            var attributeList = dataContractType.GetCustomAttributes(typeof(DataContractAttribute), false);
            if (attributeList.Length > 0)
            {
                return (DataContractAttribute)attributeList[0];
            }

            return default(DataContractAttribute);
        }

        internal static IInterceptionBehavior[] GetBehaviors(this IInterceptingProxy proxy)
        {
            return proxy.GetPipeline().GetIInterceptionBehaviors();
        }

        internal static bool IsInterfaceInterceptor(this IInterceptor proxy)
        {
            return proxy is IInstanceInterceptor;
        }

        internal static bool IsInterfaceInterceptor(this IInterceptingProxy proxy)
        {
            var fieldInfo = proxy.GetType().GetField("target", BindingFlags.NonPublic | BindingFlags.Instance);
            return fieldInfo != null;
        }

        internal static object GetTarget(this IInterceptingProxy proxy)
        {
            var fieldInfo = proxy.GetType().GetField("target", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(proxy);
            }

            throw new SerializableInterceptorException("The type {0} isn't a Interface Interceptor Proxy.", proxy.GetType().AssemblyQualifiedName);
        }

        internal static Type GetTypeToProxy(this IInterceptingProxy proxy)
        {
            var fieldInfo = proxy.GetType().GetField("typeToProxy", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return (Type)fieldInfo.GetValue(proxy);
            }

            throw new SerializableInterceptorException("The type {0} isn't a Interface Interceptor Proxy.", proxy.GetType().AssemblyQualifiedName);
        }

        private static InterceptionBehaviorPipeline GetPipeline(this IInterceptingProxy proxy)
        {
            var fieldInfo = proxy.GetType().GetField("pipeline", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return (InterceptionBehaviorPipeline)fieldInfo.GetValue(proxy);
            }

            throw new SerializableInterceptorException("The type {0} hasn't a pipeline field.", proxy.GetType().AssemblyQualifiedName);
        }
    }
}
