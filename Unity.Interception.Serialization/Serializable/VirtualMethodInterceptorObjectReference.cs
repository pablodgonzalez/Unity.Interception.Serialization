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

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    /// <summary>
    /// Surrogate for VirtualMethodInterceptor proxies.
    /// </summary>
    [Serializable]
    public sealed class VirtualMethodInterceptorObjectReference : AbstractPipelineObjectReference
    {
        private VirtualMethodInterceptorObjectReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var proxyType = new VirtualMethodInterceptor().AddSerializableSupport().CreateProxyType(TypeToProxy, AdditionalInterfaces);
            Proxy = (IInterceptingProxy)Activator.CreateInstance(
                proxyType,
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { info, context },
                null);
        }
    }
}
