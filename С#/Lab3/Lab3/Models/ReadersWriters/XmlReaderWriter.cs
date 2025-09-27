using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml.Serialization;
using Lab3.Models;

namespace Lab3.Models.ReadersWriters
{
    public class XmlReaderWriter<T> : IReaderWriter<T>
    {
        public List<T> Read(string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    return (List<T>)serializer.Deserialize(reader);
                }
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

        public void Write(List<T> items, string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (FileStream fs = new FileStream(path, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    serializer.Serialize(writer, items);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}