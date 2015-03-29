using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestBinaryFormatterUnityConfiguration : TestBinaryFormatter
    {
        private readonly IServiceLocator _container;

        public TestBinaryFormatterUnityConfiguration()
        {
            _container = new UnityServiceLocator(new UnityContainer().LoadConfiguration());
        }

        protected override IInterfaceToIntercept GetInterfaceInterceptor()
        {
            return _container.GetInstance<IInterfaceToIntercept>("interfaceinterceptorserializable");
        }

        protected override IInterfaceToIntercept GetVirtualMethodInterceptor()
        {
            return _container.GetInstance<IInterfaceToIntercept>("virtualmethodinterceptorserializable");
        }
    }
}