using System.IO;
using CsvHelper.Configuration;

namespace Lab3.Models.ReadersWriters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        public int Order { get; }
        public PropertyOrderAttribute(int order)
        {
            Order = order;
        }
    }
    public static class ReaderWriterHelper<T> where T : class, new()
    {
        public static IReaderWriter<T> readerWriter<TMap>(string filePath, IReaderWriter<T> txtReaderWriter) 
            where TMap : ClassMap<T>
        {
            var ext = Path.GetExtension(filePath).ToLower().TrimStart('.');
            
            IReaderWriter<T> newInstance;

            switch (ext)
            {
                case "txt":
                    newInstance = txtReaderWriter;
                    break;
                case "csv":
                    newInstance = new CsvReaderWriter<T, TMap>();
                    break;
                case "xml":
                    newInstance = new XmlReaderWriter<T>();
                    break;
                case "json":
                    newInstance = new JsonReaderWriter<T>();
                    break;
                case "xlsx":
                    newInstance = new ExcelReaderWriter<T>();
                    break;
                default:
                    throw new NotSupportedException($"Формат файлу '{ext}' не підтримується.");
            }
            return newInstance; 
        }
    }
}
