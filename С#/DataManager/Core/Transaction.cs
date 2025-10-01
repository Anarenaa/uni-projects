using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Transaction
    {
        public string TransactionID { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Location { get; set; }
        public string MerchantID { get; set; }
        public string Channel { get; set; }
        public int TransactionDuration { get; set; }
        public DateTime PreviousTransactionDate { get; set; }

        public Account Account { get; set; }
        public Customer Customer { get; set; }
        public Device Device { get; set; }
    }
}
