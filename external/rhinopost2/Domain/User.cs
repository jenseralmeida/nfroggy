namespace Domain
{
    public class User
    {
        private int _age;
        private string _bankAccount;
        private string _userName;

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public string BankAccount
        {
            get { return _bankAccount; }
            set { _bankAccount = value; }
        }

        public string Name
        {
            get { return _userName; }
            set { _userName = value; }
        }
    }
}