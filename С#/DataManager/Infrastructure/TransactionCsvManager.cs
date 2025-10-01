using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Core;
using CsvHelper;
using CsvHelper.Configuration;

namespace Infrastructure
{
    public class TransactionCsvManager : ITransactionManager
    {
        private CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        public List<Transaction> Read(string path)
        {
            var records = new List<Transaction>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<TransactionMap>();
                records = csv.GetRecords<Transaction>().ToList();
            }
            return records;
        }

        public void Write(string path, List<Transaction> transactions)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.Context.RegisterClassMap<TransactionMap>();
                csv.WriteRecords(transactions);
            }
        }
    }
}
