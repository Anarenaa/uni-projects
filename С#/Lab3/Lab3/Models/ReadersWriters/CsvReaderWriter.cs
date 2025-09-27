using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;

namespace Lab3.Models.ReadersWriters
{

    public class CsvReaderWriter<T, TMap> : IReaderWriter<T> where TMap : ClassMap<T>
    {
        public List<T> Read(string path)
        {
            List<T> items = new List<T>();
            try
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TMap>();
                    items = csv.GetRecords<T>().ToList();
                }
                return items;
            }
            catch(FormatException ex)
            {
                throw new FormatException($"The file is not in the correct format.\n" +
                    $"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Write(List<T> items, string path)
        {
            try
            {
                using (var writer = new StreamWriter(path, false, Encoding.Unicode))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<TMap>();
                    csv.WriteRecords(items);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
