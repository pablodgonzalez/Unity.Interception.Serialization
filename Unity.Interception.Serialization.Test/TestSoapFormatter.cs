using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using NUnit.Framework;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [TestFixture]
    public class TestSoapFormatter : TestBaseFormatter
    {
        protected override string GetVMFileName()
        {
            return "dataVM.soap";
        }

        protected override string GetIIFileName()
        {
            return "dataII.soap";
        }

        protected override IFormatter GetFormatter()
        {
            return new SoapFormatter();
        }
    }
}