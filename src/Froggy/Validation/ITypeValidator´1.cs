namespace Froggy.Validation
{
    public interface ITypeValidator<T>
    {
        bool IsNullable
        {
            get;
            set;
        }

        bool Execute(object value, out T result, out string errorMessageTemplate);
    }
}
