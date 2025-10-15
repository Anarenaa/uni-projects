using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class OperationRecord
    {
        public int RecordID { get; set; }
        public string TransactionID { get; set; }
        public string OperationName { get; set; }
        public DateTime OperationDateTime { get; set; }

        public override string ToString()
        {
            return $"{TransactionID}: {OperationName}    -   {OperationDateTime}";
        }

    }
}
