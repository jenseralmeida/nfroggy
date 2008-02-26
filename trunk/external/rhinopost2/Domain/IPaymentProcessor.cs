using System;

namespace Domain
{
    public interface IPaymentProcessor
    {
        event EventHandler<EventArgs> PaymentSucceed;   
        double GetBankBalance(string bankAccountNumber);
        void MakePayment(string bankAccountNumber);
    }
}