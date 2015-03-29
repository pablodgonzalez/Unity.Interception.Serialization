using System.IO;
using NUnit.Framework;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    public abstract class TestBaseSerialization
    {
        [TestFixtureSetUp, Category("VM")]
        public void VMFile()
        {
            IInterfaceToIntercept proxy = GetVirtualMethodInterceptor();

            proxy.PropertyBase = "SetInProxy";
            (proxy as IAdditionalInterfaceToIntercept).AdditionalProperty = "SetAddditionalInProxy";

            using (var stream = new FileStream(GetVMFileName(), FileMode.Create))
            {
                SerializeVirtualMethodInterceptor(stream, proxy);

                stream.Close();
            }
        }

        [TestFixtureSetUp, Category("II")]
        public void CrearArchivos()
        {
            IInterfaceToIntercept proxy = GetInterfaceInterceptor();

            proxy.PropertyBase = "SetInProxy";
            (proxy as IAdditionalInterfaceToIntercept).AdditionalProperty = "SetAddditionalInProxy";

            using (var stream = new FileStream(GetIIFileName(), FileMode.Create))
            {
                SerializeInterfaceInterceptor(stream, proxy);

                stream.Close();
            }
        }

        [Test, Category("II")]
        public void TestInterfaceInterceptor()
        {
            IInterfaceToIntercept proxy;

            using (var stream = new FileStream(GetIIFileName(), FileMode.Open))
            {
                proxy = DeserializeInterfaceInterceptor(stream);

                stream.Close();
            }

            File.Delete(GetIIFileName());

            Assert.IsTrue(proxy.IsIntercept());
            Assert.IsTrue(proxy is IAdditionalInterfaceToIntercept);
            Assert.IsTrue((proxy as IAdditionalInterfaceToIntercept).CanInterceptThis());
            Assert.AreEqual("SetInProxy", proxy.PropertyBase);
            Assert.AreEqual("SetAddditionalInProxy", (proxy as IAdditionalInterfaceToIntercept).AdditionalProperty);
        }

        [Test, Category("VM")]
        public void TestVirtualMethodInterceptor()
        {
            IInterfaceToIntercept proxy;

            using (var stream = new FileStream(GetVMFileName(), FileMode.Open))
            {
                proxy = DeserializeVirtualMethodInterceptor(stream);

                stream.Close();
            }

            File.Delete(GetVMFileName());

            Assert.IsTrue(proxy.IsIntercept());
            Assert.IsTrue(proxy is IAdditionalInterfaceToIntercept);
            Assert.IsTrue((proxy as IAdditionalInterfaceToIntercept).CanInterceptThis());
            Assert.AreEqual("SetInProxy", proxy.PropertyBase);
            Assert.AreEqual("SetAddditionalInProxy", (proxy as IAdditionalInterfaceToIntercept).AdditionalProperty);
        }

        protected abstract string GetVMFileName();
        protected abstract string GetIIFileName();
        protected abstract IInterfaceToIntercept GetVirtualMethodInterceptor();
        protected abstract void SerializeVirtualMethodInterceptor(Stream stream, IInterfaceToIntercept proxy);
        protected abstract IInterfaceToIntercept DeserializeVirtualMethodInterceptor(Stream stream);
        protected abstract IInterfaceToIntercept GetInterfaceInterceptor();
        protected abstract void SerializeInterfaceInterceptor(Stream stream, IInterfaceToIntercept proxy);
        protected abstract IInterfaceToIntercept DeserializeInterfaceInterceptor(Stream stream);
    }
}