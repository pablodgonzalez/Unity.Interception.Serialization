// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Create a TypeInterceptor which allow create serializable types.
        /// </summary>
        /// <param name="typeInterceptor">A <see cref="ITypeInterceptor"/>.</param>
        /// <returns>A <see cref="ITypeInterceptor"/> which allow create serializable types.</returns>
        public static ITypeInterceptor AddSerializableSupport(this ITypeInterceptor typeInterceptor)
        {
            return Intercept.ThroughProxy(typeInterceptor, new InterfaceInterceptor(), new[] { new VirtualMethodInterceptorBehavior() });
        }

        /// <summary>
        /// Create a IInstanceInterceptor which allow create serializable types.
        /// </summary>
        /// <param name="typeInterceptor">A <see cref="IInstanceInterceptor"/>.</param>
        /// <returns>A <see cref="IInstanceInterceptor"/> which allow create serializable types.</returns>
        public static IInstanceInterceptor AddSerializableSupport(this IInstanceInterceptor instanceInterceptor)
        {
            return Intercept.ThroughProxy(instanceInterceptor, new InterfaceInterceptor(), new[] { new InterfaceInterceptorBehavior() });
        }

        // Unity interception extention don't sing dynamic assemblies.
        public static string[] GetAdditionalInterfacesString(this IInterceptionBehavior[] behaviors)
        {
            return behaviors.GetAdditionalInterfaces().Select(t => t.AssemblyQualifiedName).ToArray();
        }
    }
}
