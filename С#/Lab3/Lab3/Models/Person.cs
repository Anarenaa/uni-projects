using Lab3.Models.ReadersWriters;

namespace Lab3.Models
{
    public abstract class Person : IWithId
    {
        [PropertyOrder(1)]
        public int Id { get; set; }

        [PropertyOrder(2)]
        public string? PhotoPath { get; set; }

        [PropertyOrder(3)]
        public string Name { get; set; }

        [PropertyOrder(5)]
        public string Surname { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}
