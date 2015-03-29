using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.DataContract;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestDataContractUnityFluentConfiguration : TestDataContractSerializer
    {
        private readonly IServiceLocator _container;

        public TestDataContractUnityFluentConfiguration()
        {
            _container = new UnityServiceLocator(new UnityContainer()
                .AddNewExtension<Interception>()
                .RegisterType<IInterfaceToIntercept, ContractToIntercept>(
                    "virtualmethodinterceptor",
                    new InterceptionBehavior<AdditionalInterfaceToInterceptBehavior>(),
                    new DataContractInterceptor<VirtualMethodInterceptor>())
                .RegisterType<IInterfaceToIntercept, ContractToIntercept>(
                    "interfaceinterceptor",
                    new InterceptionBehavior<AdditionalInterfaceToInterceptBehavior>(),
                    new DataContractInterceptor<InterfaceInterceptor>())
                    );
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