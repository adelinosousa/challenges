using System;

namespace ClearBank.DeveloperTest.Types
{
    public abstract class AccountBase
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

        public bool CanPay(PaymentScheme paymentScheme, decimal amount) => paymentScheme switch
        {
            PaymentScheme.Bacs => AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs),
            PaymentScheme.FasterPayments => AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) && Balance > amount,
            PaymentScheme.Chaps => AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) && Status == AccountStatus.Live,
            _ => throw new ArgumentOutOfRangeException("paymentScheme"),
        };
    }
}
