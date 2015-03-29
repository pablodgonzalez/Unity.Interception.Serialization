// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    internal static class EmitHelper
    {
        internal static MethodInfo GetRegenerateProxyBehaviorsMethodInfo()
        {
            return StaticReflection.GetMethodInfo((IInterceptingProxy proxy) => TypeProxyRegenerator.RegenerateProxyBehaviors(proxy));
        }
    }
}

