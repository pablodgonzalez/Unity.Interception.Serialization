namespace Unity.InterceptionExtension.Serialization.Test.Model
{
    public interface IInterfaceToIntercept
    {
        string PropertyBase { get; set; }
        bool IsIntercept();
    }
}