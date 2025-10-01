using Core;
using CsvHelper.Configuration;

namespace Infrastructure
{
    public sealed class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.TransactionID).Name("TransactionID");
            Map(m => m.TransactionAmount).Name("TransactionAmount");
            Map(m => m.TransactionDate).Name("TransactionDate");
            Map(m => m.TransactionType).Name("TransactionType");
            Map(m => m.Location).Name("Location");
            Map(m => m.MerchantID).Name("MerchantID");
            Map(m => m.Channel).Name("Channel");
            Map(m => m.TransactionDuration).Name("TransactionDuration");
            Map(m => m.PreviousTransactionDate).Name("PreviousTransactionDate");

            References<AccountMap>(m => m.Account);
            References<CustomerMap>(m => m.Customer);
            References<DeviceMap>(m => m.Device);
        }
    }

    public sealed class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(m => m.AccountId).Name("AccountID");
            Map(m => m.AccountBalance).Name("AccountBalance");
            Map(m => m.LoginAttempts).Name("LoginAttempts");
        }
    }

    public sealed class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            Map(m => m.Age).Name("CustomerAge");
            Map(m => m.Occupation).Name("CustomerOccupation");
        }
    }

    public sealed class DeviceMap : ClassMap<Device>
    {
        public DeviceMap()
        {
            Map(m => m.DeviceId).Name("DeviceID");
            Map(m => m.IPAddress).Name("IPAddress");
        }
    }
}
