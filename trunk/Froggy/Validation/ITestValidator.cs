namespace Froggy.Validation
{
    public interface ITestValidator
    {
        bool Execute<T>(T value, object orgValue, out string errorMessageTemplate);
    }
}
