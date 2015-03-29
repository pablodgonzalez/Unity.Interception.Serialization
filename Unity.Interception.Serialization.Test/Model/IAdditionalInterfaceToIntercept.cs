using System.Runtime.Serialization;

namespace Unity.InterceptionExtension.Serialization.Test.Model
{
    public interface IAdditionalInterfaceToIntercept
    {
        [DataMember]
        string AdditionalProperty { get; set; }

        bool CanInterceptThis();
    }
}