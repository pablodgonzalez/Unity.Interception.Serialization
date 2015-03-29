using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestDataContractUnityConfiguration : TestDataContractSerializer
    {
        private readonly IServiceLocator _container;

        public TestDataContractUnityConfiguration()
        {
            _container = new UnityServiceLocator(new UnityContainer().LoadConfiguration());
        }

        protected override IInterfaceToIntercept GetVirtualMethodInterceptor()
        {
            return _container.GetInstance<IInterfaceToIntercept>("virtualmethodinterceptor");
        }

        protected override IInterfaceToIntercept GetInterfaceInterceptor()
        {
            return _container.GetInstance<IInterfaceToIntercept>("interfaceinterceptor");
        }
    }
}