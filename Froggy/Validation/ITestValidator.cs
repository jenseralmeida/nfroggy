namespace Froggy.Validation
{
    public interface ITestValidator
    {
        bool IgnoreNullValue { get; }
        bool Execute<T>(T value, object orgValue, out string errorMessageTemplate);
    }
}
