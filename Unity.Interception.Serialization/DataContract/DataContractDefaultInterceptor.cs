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
    /// Modify the behavior of DefaultInterceptor class and set custom policy in policy list.
    /// </summary>
    public class DataContractDefaultInterceptor : DefaultInterceptor
    {
        public DataContractDefaultInterceptor(IInterceptor interceptor)
            : base(interceptor)
        {
        }

        public DataContractDefaultInterceptor(Type interceptorType, string name)
            : base(interceptorType, name)
        {
        }

        public DataContractDefaultInterceptor(Type interceptorType)
            : base(interceptorType)
        {
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            base.AddPolicies(serviceType, implementationType, name, policies);

            var policy = policies.Get<ITypeInterceptionPolicy>(implementationType);
            if (policy != null)
            {
                policies.Set<ITypeInterceptionPolicy>(new DataContractInterceptorPolicy(policy), implementationType);
            }
        }
    }

    /// <summary>
    /// Modify the behavior of DefaultInterceptor class and set DataContractInterceptorPolicy in policy list.
    /// </summary>
    public class DataContractDefaultInterceptor<TInterceptor> : DataContractDefaultInterceptor where TInterceptor : IInterceptor
    {
        public DataContractDefaultInterceptor(string name)
            : base(typeof(TInterceptor), name)
        {
        }

        public DataContractDefaultInterceptor()
            : base(typeof(TInterceptor))
        {
        }
    }
}
