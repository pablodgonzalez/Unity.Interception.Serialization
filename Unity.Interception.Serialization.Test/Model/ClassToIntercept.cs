using System;

namespace Unity.InterceptionExtension.Serialization.Test.Model
{
    [Serializable]
    public class ClassToIntercept : IInterfaceToIntercept
    {
        #region IInterfaceToIntercept Members

        public virtual bool IsIntercept()
        {
            return false;
        }

        public virtual string PropertyBase { get; set; }

        #endregion
    }
}