using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.Serializable;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestBinaryFormatterUnityFluentConfiguration : TestBinaryFormatter
    {
        private readonly IServiceLocator _container;

        public TestBinaryFormatterUnityFluentConfiguration()
        {
            _container = new UnityServiceLocator(new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterType<IInterfaceToIntercept, ClassToIntercept>(
                    "virtualmethodinterceptor",
                    new InterceptionBehavior<AdditionalInterfaceToInterceptBehavior>(),
                    new SerializableInterceptor<VirtualMethodInterceptor>())
                .RegisterType<IInterfaceToIntercept, ClassToIntercept>(
                    "interfaceinterceptor",
                    new InterceptionBehavior<AdditionalInterfaceToInterceptBehavior>(),
                    new SerializableInterceptor<InterfaceInterceptor>())
                    );
        }

        protected override IInterfaceToIntercept GetInterfaceInterceptor()
        {
            return _container.GetInstance<IInterfaceToIntercept>("interfaceinterceptor");
        }

        protected override IInterfaceToIntercept GetVirtualMethodInterceptor()
        {
            return _container.GetInstance<IInterfaceToIntercept>("virtualmethodinterceptor");
        }
    }
}