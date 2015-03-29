using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestBinaryFormatter : TestBaseFormatter
    {
        protected override IFormatter GetFormatter()
        {
            return new BinaryFormatter();
        }

        protected override string GetVMFileName()
        {
            return "dataVM.bin";
        }

        protected override string GetIIFileName()
        {
            return "dataII.bin";
        }
    }
}