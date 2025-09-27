using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Lab3.Models.ReadersWriters
{
    public class JsonReaderWriter<T> : IReaderWriter<T>
    {
        public List<T> Read(string path)
        {
            try
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                var list = JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (list == null)
                    throw new JsonException("Десеріалізація повернула null");

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

        public void Write(List<T> items, string path)
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
