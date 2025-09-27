namespace Lab3.Models.ReadersWriters
{
    public interface IReaderWriter<T>
    {
        public void Write(List<T> items, string path);
        List<T> Read(string path);
    }
}
