// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.Serializable
{
    internal abstract class AbstractSerializationSupportBehavior : IInterceptionBehavior
    {
        public virtual IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            return getNext().Invoke(input, getNext);
        }

        public abstract IEnumerable<Type> GetRequiredInterfaces();

        public bool WillExecute
        {
            get { return true; }
        }

        #region serialization Support

        protected void AddSerializationImplementation(Type typeToDerive, TypeBuilder typeBuilder, FieldBuilder proxyInterceptionPipelineField)
        {
            // not verify if the type is serializable. This is done at runtime by default mechanisms
            AddSerializableAttribute(typeBuilder);
            AddConstructorSerializable(typeToDerive, typeBuilder, proxyInterceptionPipelineField);
            AddGetObjectDataMethod(typeToDerive, typeBuilder, proxyInterceptionPipelineField);
        }

        protected abstract void InitializeProxy(ILGenerator il, Type typeToDerive, TypeBuilder typeBuilder);

        protected abstract void LoadDataObject(ILGenerator il, Type typeToDerive, TypeBuilder typeBuilder);

        private void AddSerializableAttribute(TypeBuilder typeBuilder)
        {
            var serializableConstructor = typeof(SerializableAttribute).GetConstructor(new Type[0]);
            if (serializableConstructor == null)
                throw new SerializableInterceptorException("Not Found ctor for SerializableAttribute.");
            var customAttributeBuilder = new CustomAttributeBuilder(serializableConstructor, new object[0]);
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        private void AddConstructorSerializable(Type typeToDerive, TypeBuilder typeBuilder, FieldBuilder proxyInterceptionPipelineField)
        {
            const MethodAttributes constructorAttributes = MethodAttributes.Family |
                                                           MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                                                           MethodAttributes.RTSpecialName;

            var paramTypes = new[] { typeof(SerializationInfo), typeof(StreamingContext) };
            var constructor = typeBuilder.DefineConstructor(constructorAttributes, CallingConventions.Standard, paramTypes);

            var il = constructor.GetILGenerator();

            // call base class construtor
            var ctorInfo = typeToDerive.GetConstructor(paramTypes) ?? EmitHelper.ObjectConstructorInfo();
            il.Emit(OpCodes.Ldarg_0);
            for (var i = 0; i < ctorInfo.GetParameters().Length; ++i)
            {
                il.Emit(OpCodes.Ldarg, i + 1);
            }

            il.Emit(OpCodes.Call, ctorInfo);

            // Initialize pipeline field
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, Serialization.EmitHelper.InterceptionBehaviorPipelineConstructorInfo());
            il.Emit(OpCodes.Stfld, proxyInterceptionPipelineField);

            InitializeProxy(il, typeToDerive, typeBuilder);

            il.Emit(OpCodes.Ret);
        }

        private void AddGetObjectDataMethod(Type typeToDerive, TypeBuilder typeBuilder, FieldBuilder proxyInterceptionPipelineField)
        {
            typeBuilder.AddInterfaceImplementation(typeof(ISerializable));

            const MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig |
                                        MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;

            var paramTypes = new[] { typeof(SerializationInfo), typeof(StreamingContext) };

            var methodBuilder = typeBuilder.DefineMethod("GetObjectData", methodAttributes, typeof(void), paramTypes);

            var il = methodBuilder.GetILGenerator();

            LoadDataObject(il, typeToDerive, typeBuilder);

            // Type baseType = proxyType.BaseType;
            LocalBuilder proxyBaseType = il.DeclareLocal(typeof(Type));
            il.Emit(OpCodes.Ldtoken, typeToDerive);
            il.Emit(OpCodes.Call, EmitHelper.Type_GetTypeFromHandleMethodInfo());
            il.Emit(OpCodes.Stloc_0, proxyBaseType);

            // IInterceptionBehavior[] interceptionBehaviors = this.pipeline.GetInterceptionBehaviors();
            LocalBuilder proxyIInterceptionBehaviors = il.DeclareLocal(typeof(IInterceptionBehavior[]));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, proxyInterceptionPipelineField);
            il.EmitCall(OpCodes.Call, EmitHelper.GetIInterceptionBehaviorsMethodInfo(), null);
            il.Emit(OpCodes.Stloc_1, proxyIInterceptionBehaviors);

            // string[] additionalInterfaces = interceptionBehaviors.GetAdditionalInterfacesString();
            LocalBuilder additionalInterfaces = il.DeclareLocal(typeof(string[]));
            il.Emit(OpCodes.Ldloc_1, proxyIInterceptionBehaviors);
            il.EmitCall(OpCodes.Call, EmitHelper.GetAdditionalInterfacesStringMethodInfo(), null);
            il.Emit(OpCodes.Stloc_2, additionalInterfaces);

            // info.AddValue("typeToDerive", typeToDerive.AssemblyQualifiedName);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, "typeToProxy");
            il.Emit(OpCodes.Ldloc_0, proxyBaseType);
            il.Emit(OpCodes.Callvirt, EmitHelper.Type_get_AssemblyQualifiedNameMethodInfo());
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_AddValueMethodInfo());

            //info.AddValue("interceptionBehaviors", interceptionBehaviors);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, "interceptionBehaviors");
            il.Emit(OpCodes.Ldloc_1, proxyIInterceptionBehaviors);
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_AddValueMethodInfo());

            //info.AddValue("additionalInterfaces", additionalInterfaces);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldstr, "additionalInterfaces");
            il.Emit(OpCodes.Ldloc_2, additionalInterfaces);
            il.Emit(OpCodes.Callvirt, EmitHelper.SerializationInfo_AddValueMethodInfo());



            il.Emit(OpCodes.Ret);

        }
        #endregion
    }
}
