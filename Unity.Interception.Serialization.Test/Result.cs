using System;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.InterceptionExtension.Serialization.Serializable;
using Unity.InterceptionExtension.Serialization.Test.Model;

namespace Unity.InterceptionExtension.Serialization.Test
{
    [Serializable]
    class Result : ISerializable
    {
        private readonly InterceptionBehaviorPipeline pipeline;
        private object target;


        protected Result(SerializationInfo info1, StreamingContext context)
        {
            pipeline = new InterceptionBehaviorPipeline();
            this.target = info1.GetValue("target", typeof(object));
        }

        #region Implementation of ISerializable

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. 
        ///                 </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. 
        ///                 </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. 
        ///                 </exception>
        public void GetObjectData(SerializationInfo info1, StreamingContext context)
        {
            info1.SetType(typeof(InterfaceInterceptorObjectReference));
            info1.AddValue("target", this.target);
            Type type = typeof(IInterfaceToIntercept);
            IInterceptionBehavior[] iInterceptionBehaviors = this.pipeline.GetIInterceptionBehaviors();
            string[] additionalInterfacesString = iInterceptionBehaviors.GetAdditionalInterfacesString();
            info1.AddValue("typeToProxy", type.AssemblyQualifiedName);
            info1.AddValue("interceptionBehaviors", iInterceptionBehaviors);
            info1.AddValue("additionalInterfaces", additionalInterfacesString);

        }

        #endregion
    }
}
