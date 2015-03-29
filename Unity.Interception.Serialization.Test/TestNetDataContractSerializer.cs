using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.Serializable;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestNetDataContractSerializer : TestBaseXmlObjectSerializer
    {
        protected override string GetVMFileName()
        {
            return "dataVM.net";
        }

        protected override string GetIIFileName()
        {
            return "dataII.net";
        }

        protected override IInterfaceToIntercept GetVirtualMethodInterceptor()
        {
            return Intercept.NewInstance<ClassToIntercept>(
                new VirtualMethodInterceptor().AddSerializableSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });
        }

        protected override IInterfaceToIntercept GetInterfaceInterceptor()
        {
            return Intercept.ThroughProxy<IInterfaceToIntercept>(
                new ContractToIntercept(),
                new InterfaceInterceptor().AddSerializableSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });
        }

        protected override XmlObjectSerializer GetVirtualMethodInterceptorSerializer()
        {
            return new NetDataContractSerializer();
        }

        protected override XmlObjectSerializer GetInterfaceInterceptorSerializer()
        {
            return new NetDataContractSerializer();
        }
    }
}