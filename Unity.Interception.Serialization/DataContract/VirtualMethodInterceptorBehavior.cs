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
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    /// <summary>
    /// This behavior intercept the VirtualMethodInterceptor.
    /// </summary>
    internal sealed class VirtualMethodInterceptorBehavior : IInterceptionBehavior
    {
        private static readonly IDictionary DerivedClasses = new VirtualMethodInterceptor().GetDerivedClasses();

        #region IInterceptionBehavior

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new[] { typeof(ITypeInterceptor) };
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.Name == "CreateProxyType")
            {
                var returnValue = CreateDataContractProxyType((IInterceptor)input.Target, (Type)input.Arguments[0], (Type[])input.Arguments[1]);
                return new VirtualMethodReturn(input, returnValue, null);
            }

            return getNext().Invoke(input, getNext);
        }

        public bool WillExecute
        {
            get { return true; }
        }

        #endregion

        // This method replicate the logic of original interceptor.
        private Type CreateDataContractProxyType(IInterceptor interceptor, Type typeToDerive, params Type[] additionalInterfaces)
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

                    AddDataContractAttribute(typeBuilder);
                    AddIsSurrogatedProperty(typeBuilder);
                    AddOnDeserializingMethod(typeBuilder, proxyInterceptionPipelineField);

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

        private static void AddDataContractAttribute(TypeBuilder typeBuilder)
        {
            var dataContractAttributeType = typeof(DataContractAttribute);
            var dataContractAttributeConstructor = dataContractAttributeType.GetConstructor(Type.EmptyTypes);
            var dataContractAttributePropertyNamespace = dataContractAttributeType.GetProperty("Namespace");
            var dataContractAttributePropertyName = dataContractAttributeType.GetProperty("Name");

            if (dataContractAttributeConstructor == null)
                throw new SerializableInterceptorException("Not Found ctor for DataContractAttribute.");

            var customAttributeBuilder = new CustomAttributeBuilder(
                dataContractAttributeConstructor,
                new object[0],
                new[] { dataContractAttributePropertyNamespace, dataContractAttributePropertyName },
                new object[] { Const.Namespace, Const.Proxy });
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        private void AddOnDeserializingMethod(TypeBuilder typeBuilder, FieldBuilder proxyInterceptionPipelineField)
        {
            const MethodAttributes methodAttributes = MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot;

            var paramTypes = new[] { typeof(StreamingContext) };

            var methodBuilder = typeBuilder.DefineMethod("OnDeserializing", methodAttributes, typeof(void), paramTypes);

            var onDeserializingAttributeConstructor = typeof(OnDeserializingAttribute).GetConstructor(Type.EmptyTypes);
            if (onDeserializingAttributeConstructor == null)
                throw new SerializableInterceptorException("Not Found ctor for OnDeserializingAttribute.");

            var onDeserializingAttributeBuilder = new CustomAttributeBuilder(onDeserializingAttributeConstructor, new object[0]);

            methodBuilder.SetCustomAttribute(onDeserializingAttributeBuilder);


            var il = methodBuilder.GetILGenerator();

            // pipeline = new InterceptionBehaviorPipeline();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, Serialization.EmitHelper.InterceptionBehaviorPipelineConstructorInfo());
            il.Emit(OpCodes.Stfld, proxyInterceptionPipelineField);

            // ProxyRegenerator.RegenerateProxyBehaviors(pipeline);
            il.Emit(OpCodes.Ldarg_0);
            //il.Emit(OpCodes.Ldfld, proxyInterceptionPipelineField);
            il.EmitCall(OpCodes.Call, EmitHelper.GetRegenerateProxyBehaviorsMethodInfo(), null);

            il.Emit(OpCodes.Ret);
        }

        private static void AddIsSurrogatedProperty(TypeBuilder typeBuilder)
        {
            var dataContractSupport = typeof(IDataContractSupport);
            var isSurrogated = dataContractSupport.GetProperty("IsSurrogated");

            typeBuilder.AddInterfaceImplementation(typeof(@IDataContractSupport));
            var @field = typeBuilder.DefineField("isSurrogated", isSurrogated.PropertyType, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(isSurrogated.Name, PropertyAttributes.None, isSurrogated.PropertyType, null);

            const MethodAttributes methodAtts = MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                                                MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;

            // GET
            var get = isSurrogated.GetGetMethod();
            var methodBuilder = typeBuilder.DefineMethod(get.Name, methodAtts, get.CallingConvention, get.ReturnType, Type.EmptyTypes);


            ILGenerator il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, @field);
            il.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, get);
            propertyBuilder.SetGetMethod(methodBuilder);

            // SET
            var set = isSurrogated.GetSetMethod();
            methodBuilder = typeBuilder.DefineMethod(set.Name, methodAtts, set.CallingConvention, set.ReturnType, new[] { isSurrogated.PropertyType });

            ILGenerator ilSet = methodBuilder.GetILGenerator();
            ilSet.Emit(OpCodes.Ldarg_0);
            ilSet.Emit(OpCodes.Ldarg_1);
            ilSet.Emit(OpCodes.Stfld, @field);
            ilSet.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, set);
            propertyBuilder.SetSetMethod(methodBuilder);
        }
    }
}
