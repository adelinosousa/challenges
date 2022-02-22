using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NSubstitute;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private IAccountDataStoreFactory _accountDataStoreFactory;
        private IAccountDataStore _accountDataStore;

        private PaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _accountDataStoreFactory = Substitute.For<IAccountDataStoreFactory>();
            _accountDataStore = Substitute.For<IAccountDataStore>();

            _paymentService = new PaymentService(_accountDataStoreFactory);
        }

        [Test]
        public void Bacs_Payment_PaymentSchemeNoAllowed()
        {
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs,
                DebtorAccountNumber = "12345"
            };

            _accountDataStoreFactory.GetAccountDataStore().Returns(_accountDataStore);
            _accountDataStore.GetAccount(request.DebtorAccountNumber).Returns(new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            });

            var response = _paymentService.MakePayment(request);

            Assert.IsFalse(response.Success);
        }

        [Test]
        public void Payment_InvalidPaymentScheme()
        {
            var request = new MakePaymentRequest
            {
                PaymentScheme = (PaymentScheme)99,
                DebtorAccountNumber = "12345"
            };

            _accountDataStoreFactory.GetAccountDataStore().Returns(_accountDataStore);
            _accountDataStore.GetAccount(request.DebtorAccountNumber).Returns(new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments,
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => _paymentService.MakePayment(request));
        }

        [TestCase(PaymentScheme.Bacs, AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.Chaps, AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments)]
        [TestCase(PaymentScheme.FasterPayments, AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps)]
        public void Payment_PaymentSchemeNoAllowed(PaymentScheme paymentScheme, AllowedPaymentSchemes allowedPaymentSchemes)
        {
            var request = new MakePaymentRequest
            {
                PaymentScheme = paymentScheme,
                DebtorAccountNumber = "12345"
            };

            _accountDataStoreFactory.GetAccountDataStore().Returns(_accountDataStore);
            _accountDataStore.GetAccount(request.DebtorAccountNumber).Returns(new Account
            {
                AllowedPaymentSchemes = allowedPaymentSchemes,
            });

            var response = _paymentService.MakePayment(request);

            Assert.IsFalse(response.Success);
        }

        [Test]
        public void Payment_FasterPayments_InsufficientBalance()
        {
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
                DebtorAccountNumber = "12345",
                Amount = 1
            };

            _accountDataStoreFactory.GetAccountDataStore().Returns(_accountDataStore);
            _accountDataStore.GetAccount(request.DebtorAccountNumber).Returns(new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments,
                Balance = 0
            });

            var response = _paymentService.MakePayment(request);

            Assert.IsFalse(response.Success);
        }

        [Test]
        public void Payment_FasterPayments_SufficientBalance()
        {
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
                DebtorAccountNumber = "12345",
                Amount = 1
            };

            _accountDataStoreFactory.GetAccountDataStore().Returns(_accountDataStore);
            _accountDataStore.GetAccount(request.DebtorAccountNumber).Returns(new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments,
                Balance = 2
            });

            var response = _paymentService.MakePayment(request);

            Assert.IsTrue(response.Success);
        }

        [TestCase(AccountStatus.Disabled, false)]
        [TestCase(AccountStatus.InboundPaymentsOnly, false)]
        [TestCase(AccountStatus.Live, true)]
        public void Payment_Chaps_CheckAccountStatus(AccountStatus accountStatus, bool expectedResult)
        {
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Chaps,
                DebtorAccountNumber = "12345",
            };

            _accountDataStoreFactory.GetAccountDataStore().Returns(_accountDataStore);
            _accountDataStore.GetAccount(request.DebtorAccountNumber).Returns(new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments,
                Status = accountStatus
            });

            var response = _paymentService.MakePayment(request);

            Assert.AreEqual(response.Success, expectedResult);
        }
    }
}
