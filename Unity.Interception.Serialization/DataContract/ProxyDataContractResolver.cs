// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using System.Xml;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    /// <summary>
    /// This resolver add to known type list the proxy surrogate and proxies data contract types.
    /// </summary>
    public sealed class ProxyDataContractResolver : DataContractResolver
    {
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (typeNamespace.Equals(Const.Namespace))
            {
                if (typeName.Equals(Const.ProxySurrogate)) // is proxySurrogate?
                {
                    return typeof(ProxySurrogate);
                }

                if (Const.Proxy.Equals(typeName)) // is Type Interceptor?
                {
                    return TypeProxyRegenerator.RegenerateProxyType();
                }
            }

            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
        }

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (typeof(IDataContractSupport).IsAssignableFrom(type) || typeof(ProxySurrogate).IsAssignableFrom(type))
            {
                var datacontractconfig = type.GetDataContractConfig();
                var dictionary = new XmlDictionary();
                typeName = dictionary.Add(datacontractconfig.Name);
                typeNamespace = dictionary.Add(datacontractconfig.Namespace);
                return true;
            }

            return knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
        }
    }
}
