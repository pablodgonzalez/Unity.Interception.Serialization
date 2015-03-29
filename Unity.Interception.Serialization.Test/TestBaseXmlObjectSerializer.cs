using System.IO;
using System.Runtime.Serialization;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    public abstract class TestBaseXmlObjectSerializer : TestBaseSerialization
    {
        protected abstract override IInterfaceToIntercept GetVirtualMethodInterceptor();

        protected abstract override IInterfaceToIntercept GetInterfaceInterceptor();

        protected abstract XmlObjectSerializer GetVirtualMethodInterceptorSerializer();

        protected abstract XmlObjectSerializer GetInterfaceInterceptorSerializer();

        protected abstract override string GetVMFileName();

        protected abstract override string GetIIFileName();

        protected override sealed void SerializeVirtualMethodInterceptor(Stream stream, IInterfaceToIntercept proxy)
        {
            XmlObjectSerializer serializer = GetVirtualMethodInterceptorSerializer();
            serializer.WriteObject(stream, proxy);
        }

        protected override sealed IInterfaceToIntercept DeserializeVirtualMethodInterceptor(Stream stream)
        {
            XmlObjectSerializer serializer = GetVirtualMethodInterceptorSerializer();
            return (IInterfaceToIntercept)serializer.ReadObject(stream);
        }

        protected override sealed void SerializeInterfaceInterceptor(Stream stream, IInterfaceToIntercept proxy)
        {
            XmlObjectSerializer serializer = GetInterfaceInterceptorSerializer();
            serializer.WriteObject(stream, proxy);
        }

        protected override sealed IInterfaceToIntercept DeserializeInterfaceInterceptor(Stream stream)
        {
            XmlObjectSerializer serializer = GetInterfaceInterceptorSerializer();
            return (IInterfaceToIntercept)serializer.ReadObject(stream);
        }
    }
}