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

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    /// <summary>
    /// IObjectReferenceBase for surrogate proxies.
    /// </summary>
    [Serializable]
    public abstract class AbstractPipelineObjectReference : IObjectReference, ISerializable
    {
        private readonly IInterceptionBehavior[] _behaviors;
        protected IInterceptingProxy Proxy;
        protected readonly Type TypeToProxy;
        protected readonly Type[] AdditionalInterfaces;

        protected AbstractPipelineObjectReference(SerializationInfo info, StreamingContext context)
        {
            // Deserialize the behaviors
            _behaviors = (IInterceptionBehavior[])info.GetValue("interceptionBehaviors", typeof(IInterceptionBehavior[]));

            // Deserialize the base type using its assembly qualified name
            TypeToProxy = Type.GetType(info.GetString("typeToProxy"), true, false);
            // Deserialize the additional interfacespe using its assembly qualified name
            AdditionalInterfaces = (info.GetValue("additionalInterfaces", typeof(string[])) as string[]).Select(Type.GetType).ToArray();
        }

        public object GetRealObject(StreamingContext context)
        {
            return Proxy;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// The behaviors are loaded after these have been deserialized
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            foreach (var behavior in _behaviors)
            {
                Proxy.AddInterceptionBehavior(behavior);
            }
        }
    }
}