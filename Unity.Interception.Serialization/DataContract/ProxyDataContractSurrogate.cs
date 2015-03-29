// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    /// <summary>
    /// Surrogate data contract types and interface interceptors.
    /// </summary>
    public sealed class ProxyDataContractSurrogate : IDataContractSurrogate
    {
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public Type GetDataContractType(Type type)
        {
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            if (!(obj is ProxySurrogate) && !(obj is ProxySurrogate.ProxySetting)) return obj;

            var proxySetting = obj as ProxySurrogate.ProxySetting;
            if (proxySetting != null && proxySetting.IsTypeInterceptor)
            {
                TypeProxyRegenerator.RegisterConfiguration(proxySetting);
                return proxySetting;
            }

            var proxySurrogate = obj as ProxySurrogate;
            if (proxySurrogate != null)
            {
                if (proxySurrogate.ConfigurationProxy.IsTypeInterceptor)
                {
                    return proxySurrogate.DTO;
                }

                return Intercept.ThroughProxyWithAdditionalInterfaces(
                    proxySurrogate.ConfigurationProxy.RealType,
                    proxySurrogate.DTO,
                    new InterfaceInterceptor(),
                    proxySurrogate.ConfigurationProxy.Behaviors,
                    proxySurrogate.ConfigurationProxy.AdditionalInterfaces);
            }

            return obj;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            var proxy = obj as IInterceptingProxy;

            if (proxy == null)
            {
                return obj;
            }

            var virtualproxy = (obj as IDataContractSupport);

            if (virtualproxy != null && virtualproxy.IsSurrogated)
            {
                return obj;
            }

            var proxySurrogate = new ProxySurrogate();
            proxySurrogate.ConfigurationProxy.Behaviors = proxy.GetBehaviors();
            proxySurrogate.ConfigurationProxy.AdditionalInterfaces = proxySurrogate.ConfigurationProxy.Behaviors.GetAdditionalInterfaces();

            if (virtualproxy != null)
            {
                proxySurrogate.ConfigurationProxy.IsTypeInterceptor = true;
                virtualproxy.IsSurrogated = true;
                proxySurrogate.DTO = obj;
                proxySurrogate.ConfigurationProxy.RealType = virtualproxy.GetType().BaseType;
            }
            else
            {
                proxySurrogate.DTO = proxy.GetTarget();
                proxySurrogate.ConfigurationProxy.RealType = proxy.GetTypeToProxy();
            }

            return proxySurrogate;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }
}
