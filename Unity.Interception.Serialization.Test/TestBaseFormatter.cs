using System.IO;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.InterceptionExtension.Serialization.Serializable;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    public abstract class TestBaseFormatter : TestBaseSerialization
    {
        protected override IInterfaceToIntercept GetVirtualMethodInterceptor()
        {
            return Intercept.NewInstance<ClassToIntercept>(
                new VirtualMethodInterceptor().AddSerializableSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });
        }

        protected override IInterfaceToIntercept GetInterfaceInterceptor()
        {
            return Intercept.ThroughProxy<IInterfaceToIntercept>(
                new ClassToIntercept(),
                new InterfaceInterceptor().AddSerializableSupport(),
                new[] { new AdditionalInterfaceToInterceptBehavior() });
        }

        protected override sealed void SerializeVirtualMethodInterceptor(Stream stream, IInterfaceToIntercept proxy)
        {
            IFormatter formatter = GetFormatter();
            formatter.Serialize(stream, proxy as ClassToIntercept);
        }

        protected override IInterfaceToIntercept DeserializeVirtualMethodInterceptor(Stream stream)
        {
            IFormatter formatter = GetFormatter();
            return (IInterfaceToIntercept)formatter.Deserialize(stream);
        }

        protected override void SerializeInterfaceInterceptor(Stream stream, IInterfaceToIntercept proxy)
        {
            IFormatter formatter = GetFormatter();
            formatter.Serialize(stream, proxy);
        }

        protected override IInterfaceToIntercept DeserializeInterfaceInterceptor(Stream stream)
        {
            IFormatter formatter = GetFormatter();
            return (IInterfaceToIntercept)formatter.Deserialize(stream);
        }

        protected abstract override string GetVMFileName();

        protected abstract override string GetIIFileName();

        protected abstract IFormatter GetFormatter();
    }
}