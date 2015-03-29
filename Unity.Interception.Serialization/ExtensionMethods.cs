// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization
{
    // Unity interception extention don't sing dynamic assemblies.
    public static class ExtensionMethods
    {
        internal static TypeBuilder GetTypeBuilder(this InterceptingClassGenerator interceptingClassGenerator)
        {
            return GetReflectedTypeBuilder(interceptingClassGenerator);
        }

        internal static TypeBuilder GetTypeBuilder(this InterfaceInterceptorClassGenerator interfaceInterceptorClassGenerator)
        {
            return GetReflectedTypeBuilder(interfaceInterceptorClassGenerator);
        }

        internal static FieldBuilder GetProxyInterceptionPipelineField(this InterceptingClassGenerator interceptingClassGenerator)
        {
            return GetReflectedProxyInterceptionPipelineField(interceptingClassGenerator);
        }

        internal static FieldBuilder GetProxyInterceptionPipelineField(this InterfaceInterceptorClassGenerator interfaceInterceptorClassGenerator)
        {
            return GetReflectedProxyInterceptionPipelineField(interfaceInterceptorClassGenerator);
        }

        internal static FieldBuilder GetTargetField(this InterfaceInterceptorClassGenerator interfaceInterceptorClassGenerator)
        {
            var fieldInfo = interfaceInterceptorClassGenerator.GetType().GetField("targetField", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                return (FieldBuilder)fieldInfo.GetValue(interfaceInterceptorClassGenerator);

            throw new SerializableInterceptorException("Not Found target field.");
        }

        internal static IDictionary GetDerivedClasses(this VirtualMethodInterceptor virtualMethodInterceptor)
        {
            var fieldInfo = virtualMethodInterceptor.GetType().GetField("derivedClasses", BindingFlags.NonPublic | BindingFlags.Static);
            if (fieldInfo != null)
            {
                return (IDictionary)fieldInfo.GetValue(virtualMethodInterceptor);
            }

            throw new SerializableInterceptorException("Error reading derived classes from interceptor.");
        }

        internal static IDictionary GetInterceptorClasses(this InterfaceInterceptor interfaceInterceptor)
        {
            var fieldInfo = interfaceInterceptor.GetType().GetField("interceptorClasses", BindingFlags.NonPublic | BindingFlags.Static);
            if (fieldInfo != null)
            {
                return (IDictionary)fieldInfo.GetValue(interfaceInterceptor);
            }

            throw new SerializableInterceptorException("Error reading interceptor classes from interceptor.");
        }

        internal static object CreateKey(this IDictionary derivedClasses, Type typeToDerive, params Type[] additionalInterfaces)
        {
            ConstructorInfo constructorInfo;

            try
            {
                constructorInfo = Assembly.Load("Microsoft.Practices.Unity.Interception").GetType("Microsoft.Practices.Unity.InterceptionExtension.GeneratedTypeKey").GetConstructor(new[] { typeof(Type), typeof(Type[]) });
                if (constructorInfo == null)
                    throw new ArgumentException("Not Found ctor for type Microsoft.Practices.Unity.InterceptionExtension.GeneratedTypeKey");

            }
            catch (Exception e)
            {

                throw new SerializableInterceptorException("Error reading derived classes from interceptor.", e);
            }

            return constructorInfo.Invoke(new object[] { typeToDerive, additionalInterfaces });
        }

        // Unity interception extention don't sing dynamic assemblies.
        public static IInterceptionBehavior[] GetIInterceptionBehaviors(this InterceptionBehaviorPipeline pipeline)
        {
            var fieldInfo = typeof(InterceptionBehaviorPipeline).GetField("interceptionBehaviors", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                var behaviors = (List<IInterceptionBehavior>)fieldInfo.GetValue(pipeline);
                // Get allbehaviors include serializable behaviors.
                // Because it performs runtime verification that the type is not serializable.
                return behaviors.ToArray();
            }

            throw new SerializableInterceptorException("Error reading behaviors from pipeline.");
        }

        internal static Type[] GetAdditionalInterfaces(this IInterceptionBehavior[] behaviors)
        {
            var interfaces = new List<Type>();
            foreach (var behavior in behaviors)
            {
                interfaces.AddRange(behavior.GetRequiredInterfaces().Where(t => !interfaces.Contains(t)));
            }

            return interfaces.ToArray();
        }

        private static TypeBuilder GetReflectedTypeBuilder(object obj)
        {
            var fieldInfo = obj.GetType().GetField("typeBuilder", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                return (TypeBuilder)fieldInfo.GetValue(obj);

            throw new SerializableInterceptorException("Not Found typeBuilder on type {0}", obj.GetType().AssemblyQualifiedName);
        }

        private static FieldBuilder GetReflectedProxyInterceptionPipelineField(object obj)
        {
            var fieldInfo = obj.GetType().GetField("proxyInterceptionPipelineField", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                return (FieldBuilder)fieldInfo.GetValue(obj);

            throw new SerializableInterceptorException("Not Found proxyInterceptionPipelineField on type {0}", obj.GetType().AssemblyQualifiedName);
        }
    }
}
