using System;

namespace Domain
{
    public class ShoppingCart
    {
        private string _userName;
        private double _cartAmount;

        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IUserRepository _userRepository;

        public ShoppingCart(string userName, IUserRepository userRepository, IPaymentProcessor paymentProcessor, double cartAmount)
        {
            _userName = userName;
            _cartAmount = cartAmount;
            _userRepository = userRepository;
            _paymentProcessor = paymentProcessor;
            _paymentProcessor.PaymentSucceed += (PaymentSucceed);
        }



        public double Amount
        {
            get { return _cartAmount; }
            set { _cartAmount = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


        public void Checkout()
        {
            User user = _userRepository.GetUser(UserName);
            if (user == null)
                throw new ArgumentException();

            double bankBalance = _paymentProcessor.GetBankBalance(user.BankAccount);
            bool userHasEnoughMoney = bankBalance > Amount;
            if (userHasEnoughMoney)
            {
                double oldAmount = Amount;
                try
                {
                    _paymentProcessor.MakePayment(user.BankAccount);
                }
                catch (TimeoutException e)
                {
                    // logger.Error("Payment time out", e);
                    Amount = oldAmount;
                }
            }
        }

        void PaymentSucceed(object sender, EventArgs e)
        {
            _userRepository.AddToUserPaymentHistory(UserName, Amount);
            Amount = 0;
        }

    }
}