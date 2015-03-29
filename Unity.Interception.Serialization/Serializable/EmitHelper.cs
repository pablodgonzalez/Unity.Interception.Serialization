// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    internal static class EmitHelper
    {
        internal static ConstructorInfo ObjectConstructorInfo()
        {
            return StaticReflection.GetConstructorInfo(() => new object());
        }

        internal static MethodInfo Type_GetTypeFromHandleMethodInfo()
        {
            return StaticReflection.GetMethodInfo((Type t) => Type.GetTypeFromHandle(t.TypeHandle));
        }

        internal static MethodInfo Type_get_AssemblyQualifiedNameMethodInfo()
        {
            return StaticReflection.GetPropertyGetMethodInfo((Type t) => t.AssemblyQualifiedName);
        }

        internal static MethodInfo SerializationInfo_SetTypeMethodInfo()
        {
            return StaticReflection.GetMethodInfo((SerializationInfo si) => si.SetType(null));
        }

        internal static MethodInfo SerializationInfo_AddValueMethodInfo()
        {
            return StaticReflection.GetMethodInfo((SerializationInfo si) => si.AddValue(null, null));
        }

        internal static MethodInfo GetIInterceptionBehaviorsMethodInfo()
        {
            return StaticReflection.GetMethodInfo((InterceptionBehaviorPipeline pipeline) => pipeline.GetIInterceptionBehaviors());
        }

        internal static MethodInfo GetAdditionalInterfacesStringMethodInfo()
        {
            return StaticReflection.GetMethodInfo((IInterceptionBehavior[] behaviors) => behaviors.GetAdditionalInterfacesString());
        }
    }
}

