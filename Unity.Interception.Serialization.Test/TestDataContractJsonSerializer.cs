using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.DataContract;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestDataContractJsonSerializer : TestBaseXmlObjectSerializer
    {
        protected override string GetVMFileName()
        {
            return "dataVM.json";
        }

        protected override string GetIIFileName()
        {
            return "dataII.json";
        }

        protected override IInterfaceToIntercept GetVirtualMethodInterceptor()
        {
            return Intercept.NewInstance<ContractToIntercept>(
                new VirtualMethodInterceptor().AddDataContractSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });
        }

        protected override IInterfaceToIntercept GetInterfaceInterceptor()
        {
            return Intercept.ThroughProxy<IInterfaceToIntercept>(
                new ContractToIntercept(),
                new InterfaceInterceptor(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });
        }

        protected override XmlObjectSerializer GetVirtualMethodInterceptorSerializer()
        {
            return new DataContractJsonSerializer(
                typeof(ContractToIntercept),
                new[]
                    {
                        typeof (AdditionalInterfaceToInterceptBehavior), typeof (ContractToIntercept),
                        typeof (ProxySurrogate)
                    },
                int.MaxValue,
                false,
                new ProxyDataContractSurrogateWithoutResolver(),
                false);
        }

        protected override XmlObjectSerializer GetInterfaceInterceptorSerializer()
        {
            return new DataContractJsonSerializer(
                typeof(IInterfaceToIntercept),
                new[]
                    {
                        typeof (ContractToIntercept), typeof (AdditionalInterfaceToInterceptBehavior),
                        typeof (ProxySurrogate)
                    },
                int.MaxValue,
                false,
                new ProxyDataContractSurrogate(),
                false);
        }
    }
}