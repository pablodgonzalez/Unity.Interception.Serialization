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
using System.Globalization;
using System.Reflection.Emit;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    /// <summary>
    /// This behavior intercept the VirtualMethodInterceptor.
    /// </summary>
    internal sealed class VirtualMethodInterceptorBehavior : AbstractSerializationSupportBehavior
    {
        private static readonly IDictionary DerivedClasses = new VirtualMethodInterceptor().GetDerivedClasses();
        #region IInterceptionBehavior

        public override IEnumerable<Type> GetRequiredInterfaces()
        {
            return new[] { typeof(ITypeInterceptor) };
        }

        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.Name == "CreateProxyType")
            {
                var returnValue = GenerateSerializableType((IInterceptor)input.Target, (Type)input.Arguments[0], (Type[])input.Arguments[1]);
                return new VirtualMethodReturn(input, returnValue, null);
            }

            return base.Invoke(input, getNext);
        }

        #endregion

        // This method replicate the logic of original interceptor.
        private Type GenerateSerializableType(IInterceptor interceptor, Type typeToDerive, params Type[] additionalInterfaces)
        {
            Guard.ArgumentNotNull(typeToDerive, "typeToDerive");
            Guard.ArgumentNotNull(additionalInterfaces, "additionalInterfaces");

            if (!interceptor.CanIntercept(typeToDerive))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The type {0} is not interceptable.", typeToDerive.Name));
            }

            Type interceptorType;
            var typeToDeriveAux = typeToDerive;
            var genericType = false;

            if (typeToDerive.IsGenericType)
            {
                typeToDeriveAux = typeToDerive.GetGenericTypeDefinition();
                genericType = true;
            }

            var key = DerivedClasses.CreateKey(typeToDeriveAux, additionalInterfaces);

            lock (DerivedClasses)
            {
                if (!DerivedClasses.Contains(key))
                {
                    var generator = new InterceptingClassGenerator(typeToDeriveAux, additionalInterfaces);

                    var typeBuilder = generator.GetTypeBuilder();

                    var proxyInterceptionPipelineField = generator.GetProxyInterceptionPipelineField();

                    AddSerializationImplementation(typeToDeriveAux, typeBuilder, proxyInterceptionPipelineField);

                    interceptorType = generator.GenerateType();
                    DerivedClasses[key] = interceptorType;
                }
                else
                    interceptorType = (Type)DerivedClasses[key];
            }

            if (genericType)
            {
                interceptorType = interceptorType.MakeGenericType(typeToDerive.GetGenericArguments());
            }

            return interceptorType;
        }

        protected override void InitializeProxy(ILGenerator il, Type typeToDerive, TypeBuilder typeBuilder)
        {
        }

        protected override void LoadDataObject(ILGenerator il, Type typeToDerive, TypeBuilder typeBuilder)
        {
            // info.SetType(typeof(VirtualMethodInterceptiorObjectReference));
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldtoken, typeof(VirtualMethodInterceptorObjectReference));
            il.Emit(OpCodes.Call, EmitHelper.Type_GetTypeFromHandleMethodInfo());
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_SetTypeMethodInfo());
        }
    }
}
