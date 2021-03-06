﻿// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration;
using Unity.InterceptionExtension.Serialization.DataContract;

namespace Unity.InterceptionExtension.Serialization.Configuration
{
    /// <summary>
    /// Configuration element for DataContractInterceptor.
    /// </summary>
    public class DataContractInterceptorElement : InterceptorElement
    {
        public override IEnumerable<InjectionMember> GetInjectionMembers(IUnityContainer container, Type fromType, Type toType, string name)
        {
            // the base class make validations!
            base.GetInjectionMembers(container, fromType, toType, name);

            if (IsDefaultForType)
            {
                return new[] { new DataContractDefaultInterceptor(TypeResolver.ResolveType(TypeName), Name) };
            }

            return new[] { new DataContractInterceptor(TypeResolver.ResolveType(TypeName), Name) };
        }
    }
}
