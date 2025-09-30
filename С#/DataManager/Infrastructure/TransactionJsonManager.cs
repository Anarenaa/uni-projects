using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Core;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class TransactionJsonManager : ITransactionManager
    {
        public List<Transaction> Read(string path)
        {
            try
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                var list = JsonConvert.DeserializeObject<List<Transaction>>(json, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                });

                if (list == null)
                    throw new JsonException("Is null");

                return list;
            }
            catch (FormatException ex)
            {
                throw new FormatException($"The file is not in the correct format.\n" +
                    $"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Write( string path, List<Transaction> items)
        {
            try
            {
                var json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(path, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
