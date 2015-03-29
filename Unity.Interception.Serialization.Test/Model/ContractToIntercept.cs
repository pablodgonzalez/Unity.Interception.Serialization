using System.Runtime.Serialization;

namespace Unity.InterceptionExtension.Serialization.Test.Model
{
    [DataContract]
    public class ContractToIntercept : IInterfaceToIntercept
    {
        private string _propertyBase = "base";

        #region IInterfaceToIntercept Members

        [DataMember]
        public virtual string PropertyBase
        {
            get { return _propertyBase; }
            set { _propertyBase = value; }
        }

        public virtual bool IsIntercept()
        {
            return false;
        }

        #endregion
    }
}