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
    /// <summary>
    /// Modify the behavior of Interceptor class and set custom policy in policy list.
    /// </summary>
    public class SerializableInterceptor : Interceptor
    {
        public SerializableInterceptor(IInterceptor interceptor)
            : base(interceptor)
        {
        }

        public SerializableInterceptor(Type interceptorType, string name)
            : base(interceptorType, name)
        {
        }

        public SerializableInterceptor(Type interceptorType)
            : base(interceptorType)
        {
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            base.AddPolicies(serviceType, implementationType, name, policies);
            var key = new NamedTypeBuildKey(implementationType, name);

            dynamic policy = policies.Get<ITypeInterceptionPolicy>(key);
            if (policy != null)
            {
                policies.Set<ITypeInterceptionPolicy>(new SerializableInterceptorPolicy(policy), key);
            }

            policy = policies.Get<IInstanceInterceptionPolicy>(key);
            if (policy != null)
            {
                policies.Set<IInstanceInterceptionPolicy>(new SerializableInterceptorPolicy(policy), key);
            }
        }
    }

    /// <summary>
    /// Modify the behavior of Interceptor class and set custom policy in policy list.
    /// </summary>
    public class SerializableInterceptor<TInterceptor> : SerializableInterceptor where TInterceptor : IInterceptor
    {
        public SerializableInterceptor(string name)
            : base(typeof(TInterceptor), name)
        {
        }

        public SerializableInterceptor()
            : base(typeof(TInterceptor))
        {
        }
    }
}
