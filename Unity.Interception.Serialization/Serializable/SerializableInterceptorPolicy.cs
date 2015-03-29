// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    internal sealed class SerializableInterceptorPolicy : ITypeInterceptionPolicy, IInstanceInterceptionPolicy
    {
        private readonly ITypeInterceptionPolicy _originalTypePolicy;
        private readonly IInstanceInterceptionPolicy _originalInterfacePolicy;

        public SerializableInterceptorPolicy(ITypeInterceptionPolicy policy)
        {
            _originalTypePolicy = policy;
        }

        public SerializableInterceptorPolicy(IInstanceInterceptionPolicy policy)
        {
            _originalInterfacePolicy = policy;
        }

        IInstanceInterceptor IInstanceInterceptionPolicy.GetInterceptor(IBuilderContext context)
        {
            return _originalInterfacePolicy.GetInterceptor(context).AddSerializableSupport();
        }

        ITypeInterceptor ITypeInterceptionPolicy.GetInterceptor(IBuilderContext context)
        {
            return _originalTypePolicy.GetInterceptor(context).AddSerializableSupport();
        }

        Type ITypeInterceptionPolicy.ProxyType
        {
            get
            {
                return _originalTypePolicy.ProxyType;
            }
            set
            {
                _originalTypePolicy.ProxyType = value;
            }
        }
    }
}
