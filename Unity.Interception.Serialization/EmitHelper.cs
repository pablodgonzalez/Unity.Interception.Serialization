// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace Unity.InterceptionExtension.Serialization
{
    internal static class EmitHelper
    {
        internal static ConstructorInfo InterceptionBehaviorPipelineConstructorInfo()
        {
            return StaticReflection.GetConstructorInfo(() => new InterceptionBehaviorPipeline());
        }
    }
}

