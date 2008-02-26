using System;
using Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace RhinoPost2
{
    [TestFixture]
    public class Test
    {
        private MockRepository _mockRepository;
        private IUserRepository _userRepository;
        private IPaymentProcessor _paymentProcessor;

        private readonly User _user = new User();

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository();
            _userRepository = _mockRepository.DynamicMock<IUserRepository>();
            _paymentProcessor = _mockRepository.DynamicMock<IPaymentProcessor>();

            SetDefaultUserData();
        }


        [TearDown]
        public void TearDown()
        {
            _mockRepository.ReplayAll();
            _mockRepository.VerifyAll();
        }



        /// <summary>
        /// This test presents usage of IgnoreArguments()
        /// Purpose of this test is to verify that if a GetUser would get ANY argument 
        /// which would cause returning of the null user value, checkout method should 
        /// throw an argument exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Checkout_Verify_That_NotKnownUser_Throws_ArgumentException()
        {

            //seting up the stage - retreival of the user would return null
            Expect.Call(_userRepository.GetUser(null))
                .IgnoreArguments()
                .Return(new User());

            //stop recording
            _mockRepository.ReplayAll();

            ShoppingCart cart = new ShoppingCart(_user.Name, _userRepository, _paymentProcessor, 0);
            cart.Checkout();
        }

        /// <summary>
        /// This test describes: 
        /// - how NUnit assert can be used on the non mocked object to verify that mocks made desired effect on it
        /// - how to simulate event raised on the mock 
        /// </summary>
        [Test]
        public void Checkout_Verify_That_Successfull_CheckOut_Would_Make_Cart_Value_Zero()
        {
            double cartAmount = 100;

            //seting up the stage 

            // 1. Regular user with default data would be retreived
            Expect.Call(_userRepository.GetUser(_user.Name))
                .Return(_user);

            //2. User would have enough money on h\is account for payment
            Expect.Call(_paymentProcessor.GetBankBalance(_user.BankAccount))
                .Return(cartAmount + 1);

            // 3. geting the payment proccessor payment succeed event raiser 
            // which we would use to simulate the async signal that the payment is complete
            _paymentProcessor.PaymentSucceed += null;
            IEventRaiser paymentSucceedEventRaiser = LastCall.IgnoreArguments().GetEventRaiser(); 

            // 4. Expect that successfull payment would be stored in user payment history
            _userRepository.AddToUserPaymentHistory(_user.Name, cartAmount);

            //stop recording
            _mockRepository.ReplayAll();

            ShoppingCart cart = new ShoppingCart(_user.Name, _userRepository, _paymentProcessor, cartAmount);
            cart.Checkout();
            // we now simulate payment suceed event send from payment processor 
            paymentSucceedEventRaiser.Raise(_paymentProcessor,new EventArgs());
            // checking if the cart amount is zero
            Assert.IsTrue(cart.Amount.Equals(0), "Cart amount to be paid not reset to zero.");
        }


        /// <summary>
        /// This test describes two things:
        /// a) it explains LastCall mechanism for handling void returning methods in Rhino Mocks
        /// b) it explains how to simulate mocked object throwing an exception scenario
        /// </summary>
        [Test]
        public void Checkout_Verify_That_In_Case_Of_Payment_Timeout_Cart_Value_Would_Stay_Unchanged()
        {
            double cartAmount = 100;

            //seting up the stage 
            
            // 1. Regular user would be retreived
            Expect.Call(_userRepository.GetUser(_user.Name))
                .Return(_user);

            //2. Account check would return information that user is having enough money
            Expect.Call(_paymentProcessor.GetBankBalance(_user.BankAccount))
                .Return(cartAmount + 1);

            _paymentProcessor.MakePayment(_user.BankAccount);
            LastCall.Throw(new TimeoutException("Pament failed. Please try later again."));

            //stop recording
            _mockRepository.ReplayAll();

            // cart amount is less then user account balance
            ShoppingCart cart = new ShoppingCart(_user.Name, _userRepository, _paymentProcessor, cartAmount);
            try
            {
                cart.Checkout();
                Assert.IsTrue(cart.Amount == cartAmount, "Cart amount is changed altought there was timeout roolback scenario");
            }
            catch (TimeoutException)
            {
                Assert.Fail("Checkout procedure didn't recover gracefully from timeout exception");
            }
        }


        /// <summary>
        /// This test describes Repeat.Never() for testing that some method WAS NOT executed in certain scenario
        /// 
        /// </summary>
        [Test]
        public void Checkout_Verify_That_User_Without_Enough_Money_Cant_Complete_CheckOut()
        {
            double cartAmount = 100;

            //seting up the stage 
            // 1. Regular user would be retreived
            Expect.Call(_userRepository.GetUser(_user.Name))
                .Return(_user);

            //2. Account check would return information that user is having enough money
            Expect.Call(_paymentProcessor.GetBankBalance(_user.BankAccount))
                .Return(cartAmount - 1);

            //expectation: MakePayment won't be called because user is not having enough money on account
            _paymentProcessor.MakePayment(null);
            LastCall.IgnoreArguments()              /* we don't care which user id should be used */
                .Repeat.Never();                    /* We expect zero method execution*/

            //stop recording
            _mockRepository.ReplayAll();

            // cart amount is +1 then user account balance
            ShoppingCart cart = new ShoppingCart(_user.Name, _userRepository, _paymentProcessor, cartAmount);
            cart.Checkout();
            Assert.IsTrue(cart.Amount == cartAmount, "Cart value was modified event the payment was not made");
        }

        /// <summary>
        /// This test describes:
        /// - Repeat.Once - How to test performance related things such as lazy load
        /// </summary>
        [Test]
        public void Checkout_Verify_That_Retreving_Of_User_Data_Occured_Only_Once()
        {
            double bankBalance = 100;
            double cartAmount = bankBalance - 1;

            //seting up the stage 
            Expect.Call(_userRepository.GetUser(_user.Name))
                .Return(_user)
                .Repeat.Once();

            //stop recording
            _mockRepository.ReplayAll();

            // cart amount is +1 then user account balance
            ShoppingCart cart = new ShoppingCart(_user.Name, _userRepository, _paymentProcessor, cartAmount);
            cart.Checkout();
        }

        /// <summary>
        /// This test describes:
        /// - Rhino mock Message method purpose 
        /// This test would be the only one failing so we could see the erro message
        /// </summary>
        [Test]
        public void Checkout_Verify_That_Retreving_Of_User_Data_Occured_Twice()
        {
            //seting up the stage 
            Expect.Call(_userRepository.GetUser(_user.Name))
                .Return(_user)
                .Repeat.Twice()
                .Message("Retreiving of the user did not occured twice");

            //stop recording
            _mockRepository.ReplayAll();

            // cart amount is +1 then user account balance
            ShoppingCart cart = new ShoppingCart(_user.Name, _userRepository, _paymentProcessor, 0);
            cart.Checkout();
        }


        private void SetDefaultUserData()
        {
            _user.Name = "Nikola";
            _user.Age = 30;
            _user.BankAccount = "12345";
        }

    }
}