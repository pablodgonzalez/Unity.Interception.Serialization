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
    /// Modify the behavior of DefaultInterceptor class and set custom policy in policy list.
    /// </summary>
    public class SerializableDefaultInterceptor : DefaultInterceptor
    {
        public SerializableDefaultInterceptor(IInterceptor interceptor)
            : base(interceptor)
        {
        }

        public SerializableDefaultInterceptor(Type interceptorType, string name)
            : base(interceptorType, name)
        {
        }

        public SerializableDefaultInterceptor(Type interceptorType)
            : base(interceptorType)
        {
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            base.AddPolicies(serviceType, implementationType, name, policies);

            var typepolicy = policies.Get<ITypeInterceptionPolicy>(implementationType);
            if (typepolicy != null)
            {
                policies.Set<ITypeInterceptionPolicy>(new SerializableInterceptorPolicy(typepolicy), implementationType);
            }

            var instancepolicy = policies.Get<IInstanceInterceptionPolicy>(implementationType);
            if (instancepolicy != null)
            {
                policies.Set<IInstanceInterceptionPolicy>(new SerializableInterceptorPolicy(instancepolicy), implementationType);
            }
        }
    }

    /// <summary>
    /// Modify the behavior of DefaultInterceptor class and set custom policy in policy list.
    /// </summary>
    public class SerializableDefaultInterceptor<TInterceptor> : SerializableDefaultInterceptor where TInterceptor : IInterceptor
    {
        public SerializableDefaultInterceptor(string name)
            : base(typeof(TInterceptor), name)
        {
        }

        public SerializableDefaultInterceptor()
            : base(typeof(TInterceptor))
        {
        }
    }
}
