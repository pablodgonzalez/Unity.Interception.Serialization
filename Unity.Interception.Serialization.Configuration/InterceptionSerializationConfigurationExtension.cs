// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using Microsoft.Practices.Unity.Configuration;

namespace Unity.InterceptionExtension.Serialization.Configuration
{
    /// <summary>
    /// Section extension for add serialization to interceptors.
    /// </summary>
    public class InterceptionSerializationConfigurationExtension : SectionExtension
    {
        public override void AddExtensions(SectionExtensionContext context)
        {
            context.AddElement<DataContractInterceptorElement>("dataContractInterceptor");
            context.AddElement<SerializableInterceptorElement>("serializableInterceptor");
        }
    }
}
