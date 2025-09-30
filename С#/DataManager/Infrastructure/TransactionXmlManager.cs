using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Core;

namespace Infrastructure
{
    public class TransactionXmlManager : ITransactionManager
    {
        public List<Transaction> Read(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Transaction>), new XmlRootAttribute("Transactions"));
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    if (stream.Length == 0)
                    {
                        return new List<Transaction>();
                    }
                    return (List<Transaction>)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading XML file: {ex.Message}", ex);
            }
        }

        public void Write(string path, List<Transaction> transactions)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Transaction>), new XmlRootAttribute("Transactions"));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(stream, transactions);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to XML file: {ex.Message}", ex);
            }
        }
    }
}
