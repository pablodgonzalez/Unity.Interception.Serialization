using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.DataContract;
using Unity.InterceptionExtension.Serialization.Serializable;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestProxy
    {
        [Test]
        public void TestInterfaceInterceptor()
        {
            var proxy = Intercept.ThroughProxy<IInterfaceToIntercept>(
                new ClassToIntercept(),
                new InterfaceInterceptor().AddSerializableSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });

            Assert.IsTrue(proxy.IsIntercept());
            Assert.IsTrue(proxy is IAdditionalInterfaceToIntercept);
            Assert.IsTrue((proxy as IAdditionalInterfaceToIntercept).CanInterceptThis());
        }

        [Test]
        public void TestVirtualMethodInterceptContract()
        {
            var proxy = Intercept.NewInstance<ContractToIntercept>(
                new VirtualMethodInterceptor().AddDataContractSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });

            Assert.IsTrue(proxy.IsIntercept());
            Assert.IsTrue(proxy is IAdditionalInterfaceToIntercept);
            Assert.IsTrue((proxy as IAdditionalInterfaceToIntercept).CanInterceptThis());
        }

        [Test]
        public void TestVirtualMethodInterceptor()
        {
            var proxy = Intercept.NewInstance<ClassToIntercept>(
                new VirtualMethodInterceptor().AddSerializableSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });

            Assert.IsTrue(proxy.IsIntercept());
            Assert.IsTrue(proxy is IAdditionalInterfaceToIntercept);
            Assert.IsTrue((proxy as IAdditionalInterfaceToIntercept).CanInterceptThis());
        }
    }
}