// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    /// <summary>
    /// Surrogate for proxies.
    /// </summary>
    [DataContract(Namespace = Const.Namespace, Name = Const.ProxySurrogate), KnownType(typeof(ProxySetting))]
    public sealed class ProxySurrogate
    {
        [DataMember(Order = 1, Name = "setting", IsRequired = true)]
        private ProxySetting _setting = new ProxySetting();

        public ProxySetting ConfigurationProxy
        {
            get
            {
                return _setting;
            }
        }

        [DataMember(Order = 2, Name = "dto", IsRequired = true)]
        public object DTO { get; set; }

        [DataContract(Namespace = Const.Namespace, Name = Const.ProxySetting)]
        public sealed class ProxySetting
        {
            [DataMember(Name = "realType", IsRequired = true)]
            private string _realType;

            [DataMember(Name = "additionalInterfaces", IsRequired = true)]
            private string[] _additionalInterfaces;

            public Type RealType
            {
                get
                {
                    return Type.GetType(_realType);
                }

                set
                {
                    _realType = value.AssemblyQualifiedName;
                }
            }

            [DataMember(Name = "behaviors", IsRequired = true)]
            public IInterceptionBehavior[] Behaviors { get; set; }

            public Type[] AdditionalInterfaces
            {
                get { return _additionalInterfaces.Select(Type.GetType).ToArray(); }
                set { _additionalInterfaces = value.Select(t => t.AssemblyQualifiedName).ToArray(); }
            }

            [DataMember]
            public bool IsTypeInterceptor { get; set; }
        }
    }
}
