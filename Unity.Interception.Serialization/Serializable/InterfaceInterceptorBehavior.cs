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
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    /// <summary>
    /// This behavior intercept the VirtualMethodInterceptor.
    /// </summary>
    internal sealed class InterfaceInterceptorBehavior : AbstractSerializationSupportBehavior
    {
        private static readonly IDictionary InterceptorClasses = new InterfaceInterceptor().GetInterceptorClasses();

        private FieldInfo _targetField;

        #region IInterceptionBehavior
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.Name == "CreateProxy")
            {
                var returnValue = GenerateSerializableProxy((Type)input.Arguments[0], input.Arguments[1], (Type[])input.Arguments[2]);
                return new VirtualMethodReturn(input, returnValue, null);
            }

            return base.Invoke(input, getNext);
        }

        public override IEnumerable<Type> GetRequiredInterfaces()
        {
            return new[] { typeof(IInstanceInterceptor) };
        }
        #endregion

        private IInterceptingProxy GenerateSerializableProxy(Type typeToProxy, object target, params Type[] additionalInterfaces)
        {
            Guard.ArgumentNotNull(typeToProxy, "typeToProxy");
            Guard.ArgumentNotNull(additionalInterfaces, "additionalInterfaces");

            Type interceptorType;
            var typeToProxyAux = typeToProxy;
            var genericType = false;

            if (typeToProxy.IsGenericType)
            {
                typeToProxyAux = typeToProxy.GetGenericTypeDefinition();
                genericType = true;
            }

            var key = InterceptorClasses.CreateKey(typeToProxyAux, additionalInterfaces);

            lock (InterceptorClasses)
            {
                if (!InterceptorClasses.Contains(key))
                {
                    var generator = new InterfaceInterceptorClassGenerator(typeToProxyAux, additionalInterfaces);

                    var typeBuilder = generator.GetTypeBuilder();

                    var proxyInterceptionPipelineField = generator.GetProxyInterceptionPipelineField();

                    _targetField = generator.GetTargetField();

                    AddSerializationImplementation(typeToProxyAux, typeBuilder, proxyInterceptionPipelineField);

                    interceptorType = generator.CreateProxyType();
                    InterceptorClasses[key] = interceptorType;
                }
                else
                    interceptorType = (Type)InterceptorClasses[key];
            }

            if (genericType)
            {
                interceptorType = interceptorType.MakeGenericType(typeToProxy.GetGenericArguments());
            }

            return (IInterceptingProxy)interceptorType.GetConstructors()[0].Invoke(new[] { target, typeToProxy });
        }

        protected override void InitializeProxy(ILGenerator il, Type typeToDerive, TypeBuilder typeBuilder)
        {
            // this.target = info.GetValue("target", targetType);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, "target");
            il.Emit(OpCodes.Ldtoken, typeof(object));
            il.Emit(OpCodes.Call, EmitHelper.Type_GetTypeFromHandleMethodInfo());
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_AddValueMethodInfo());
            il.Emit(OpCodes.Stfld, _targetField);
        }

        protected override void LoadDataObject(ILGenerator il, Type typeToDerive, TypeBuilder typeBuilder)
        {
            // info.SetType(typeof(InterfaceInterceptorObjectReference));
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldtoken, typeof(InterfaceInterceptorObjectReference));
            il.Emit(OpCodes.Call, EmitHelper.Type_GetTypeFromHandleMethodInfo());
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_SetTypeMethodInfo());

            //info.AddValue("target", target);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, "target");
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, _targetField);
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_AddValueMethodInfo());
        }
    }
}
