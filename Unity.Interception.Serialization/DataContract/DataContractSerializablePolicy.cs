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

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    internal sealed class DataContractInterceptorPolicy : ITypeInterceptionPolicy
    {
        private readonly ITypeInterceptionPolicy _originalPolicy;

        public DataContractInterceptorPolicy(ITypeInterceptionPolicy policy)
        {
            _originalPolicy = policy;
        }

        public ITypeInterceptor GetInterceptor(IBuilderContext context)
        {
            return _originalPolicy.GetInterceptor(context).AddDataContractSupport();
        }

        public Type ProxyType
        {
            get { return _originalPolicy.ProxyType; }
            set { _originalPolicy.ProxyType = value; }
        }
    }
}
