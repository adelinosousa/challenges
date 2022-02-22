using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _accountDataStoreFactory;

        public PaymentService(IAccountDataStoreFactory accountDataStoreFactory)
        {
            _accountDataStoreFactory = accountDataStoreFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountDataStore = _accountDataStoreFactory.GetAccountDataStore();

            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult();

            if (account != null && account.CanPay(request.PaymentScheme, request.Amount))
            {
                account.Balance -= request.Amount;

                accountDataStore.UpdateAccount(account);

                result.Success = true;
            }

            return result;
        }
    }
}
