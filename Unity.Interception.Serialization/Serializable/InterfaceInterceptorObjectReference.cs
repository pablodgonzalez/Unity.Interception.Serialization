// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    /// <summary>
    /// Surrogate for InterfaceInterceptor proxies.
    /// </summary>
    [Serializable]
    public sealed class InterfaceInterceptorObjectReference : AbstractPipelineObjectReference
    {
        private InterfaceInterceptorObjectReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var target = info.GetValue("target", typeof(object));

            Proxy = new InterfaceInterceptor().AddSerializableSupport().CreateProxy(TypeToProxy, target, AdditionalInterfaces);
        }
    }
}
