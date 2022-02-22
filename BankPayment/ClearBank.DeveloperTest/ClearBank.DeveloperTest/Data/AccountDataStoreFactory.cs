using System.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private readonly string _dataStoreType;

        public AccountDataStoreFactory()
        {
            _dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];
        }

        public IAccountDataStore GetAccountDataStore() => _dataStoreType switch
        {
            "Backup" => new BackupAccountDataStore(),
            _ => new AccountDataStore(),
        };
    }
}
