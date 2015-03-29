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
    /// This surrogate needs ProxySurrogate is in known type list.
    /// Proxy is replace for base class. Only public and protected fields/properties are copied.
    /// </summary>
    public sealed class ProxyDataContractSurrogateWithoutResolver : IDataContractSurrogate
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
                    return ChangeTypeToProxy(proxySurrogate);
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
                proxySurrogate.ConfigurationProxy.RealType = virtualproxy.GetType().BaseType;
                proxySurrogate.DTO = ChangeTypeToBase(obj);
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

        private object ChangeTypeToBase(object inherited)
        {
            var parent = Activator.CreateInstance(inherited.GetType().BaseType);

            CopyCommonInfo(inherited, parent);

            return parent;
        }

        private object ChangeTypeToProxy(ProxySurrogate surrogate)
        {
            var proxy = (IInterceptingProxy)Activator.CreateInstance(TypeProxyRegenerator.RegenerateProxyType());

            TypeProxyRegenerator.RegenerateProxyBehaviors(proxy);

            CopyCommonInfo(surrogate.DTO, proxy);

            return proxy;
        }

        private void CopyCommonInfo(object source, object target)
        {
            const BindingFlags fieldsflags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            const BindingFlags propertiesflags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            // protected and public fields
            foreach (var fieldInfo in target.GetType().GetFields(fieldsflags | BindingFlags.SetField))
            {
                var fieldInfoInherited = source.GetType().GetField(fieldInfo.Name, BindingFlags.GetField | fieldsflags);
                if (fieldInfoInherited != null)
                    fieldInfo.SetValue(target, fieldInfoInherited.GetValue(source));
            }

            // protected and public properties
            foreach (var propertyInfo in target.GetType().GetProperties(BindingFlags.SetProperty | propertiesflags))
            {
                var propertyInfoInherited = source.GetType().GetProperty(propertyInfo.Name, BindingFlags.GetProperty | propertiesflags);
                if (propertyInfoInherited != null)
                    propertyInfo.SetValue(target, propertyInfoInherited.GetValue(source, null), null);
            }
        }
    }
}
