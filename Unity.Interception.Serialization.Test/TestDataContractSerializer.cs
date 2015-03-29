using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.DataContract;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestDataContractSerializer : TestBaseXmlObjectSerializer
    {
        protected override string GetVMFileName()
        {
            return "dataVM.xml";
        }

        protected override string GetIIFileName()
        {
            return "dataII.xml";
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
            return new DataContractSerializer(
                typeof(ContractToIntercept),
                new[]
                    {
                        typeof (ContractToIntercept), typeof (AdditionalInterfaceToInterceptBehavior),
                        typeof (ProxySurrogate)
                    },
                int.MaxValue,
                false,
                false,
                new ProxyDataContractSurrogateWithoutResolver());
        }

        protected override XmlObjectSerializer GetInterfaceInterceptorSerializer()
        {
            return new DataContractSerializer(
                typeof(IInterfaceToIntercept),
                new[]
                    {
                        typeof (ContractToIntercept), typeof (AdditionalInterfaceToInterceptBehavior),
                        typeof (ProxySurrogate)
                    },
                int.MaxValue,
                false,
                false,
                new ProxyDataContractSurrogateWithoutResolver());
        }
    }
}