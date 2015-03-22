namespace Domain
{
    public interface IUserRepository
    {
        User GetUser(string userName);
        void AddToUserPaymentHistory(string userName, double amount);
    }
}