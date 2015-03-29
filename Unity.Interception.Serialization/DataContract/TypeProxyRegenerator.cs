// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    // Unity interception extention don't sing dynamic assemblies.
    public static class TypeProxyRegenerator
    {
        private static readonly Stack<ProxySurrogate.ProxySetting> ConfigProxies = new Stack<ProxySurrogate.ProxySetting>();

        internal static void RegisterConfiguration(ProxySurrogate.ProxySetting configuration)
        {
            ConfigProxies.Push(configuration);
        }

        internal static Type RegenerateProxyType()
        {
            if (ConfigProxies.Count == 0 || !ConfigProxies.Peek().IsTypeInterceptor)
            {
                throw new SerializationException(@"Error on regenerate type proxy. There aren't configurations for proxies or the proxy isn't a type interceptor.");
            }

            return new VirtualMethodInterceptor().AddDataContractSupport().CreateProxyType(
                ConfigProxies.Peek().RealType,
                ConfigProxies.Peek().AdditionalInterfaces);
        }

        // Unity interception extention don't sing dynamic assemblies.
        public static void RegenerateProxyBehaviors(IInterceptingProxy proxy)
        {
            if (ConfigProxies.Count > 0 && !proxy.IsInterfaceInterceptor() && ConfigProxies.Peek().IsTypeInterceptor && proxy.GetType().BaseType.IsAssignableFrom(ConfigProxies.Peek().RealType))
            {
                var config = ConfigProxies.Pop();

                foreach (var interceptionBehavior in config.Behaviors)
                {
                    proxy.AddInterceptionBehavior(interceptionBehavior);
                }
            }
        }
    }
}
