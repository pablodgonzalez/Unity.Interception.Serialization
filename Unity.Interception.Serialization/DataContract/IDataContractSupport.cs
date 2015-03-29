// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

namespace Unity.InterceptionExtension.Serialization.DataContract
{
    /// <summary>
    /// This interface identify a data contract type that can be serialize.
    /// </summary>
    public interface IDataContractSupport
    {
        bool IsSurrogated { get; set; }
    }
}
