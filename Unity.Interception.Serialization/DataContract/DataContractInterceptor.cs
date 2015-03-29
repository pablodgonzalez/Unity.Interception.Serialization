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
    /// <summary>
    /// Modify the behavior of Interceptor class and set custom policy in policy list.
    /// </summary>
    public class DataContractInterceptor : Interceptor
    {
        public DataContractInterceptor(IInterceptor interceptor)
            : base(interceptor)
        {
        }

        public DataContractInterceptor(Type interceptorType, string name)
            : base(interceptorType, name)
        {
        }

        public DataContractInterceptor(Type interceptorType)
            : base(interceptorType)
        {
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            base.AddPolicies(serviceType, implementationType, name, policies);
            var key = new NamedTypeBuildKey(implementationType, name);

            var policy = policies.Get<ITypeInterceptionPolicy>(key);
            if (policy != null)
            {
                policies.Set<ITypeInterceptionPolicy>(new DataContractInterceptorPolicy(policy), key);
            }
        }
    }

    /// <summary>
    /// Modify the behavior of Interceptor class and set custom policy in policy list.
    /// </summary>
    public class DataContractInterceptor<TInterceptor> : DataContractInterceptor where TInterceptor : IInterceptor
    {
        public DataContractInterceptor(string name)
            : base(typeof(TInterceptor), name)
        {
        }

        public DataContractInterceptor()
            : base(typeof(TInterceptor))
        {
        }
    }
}
