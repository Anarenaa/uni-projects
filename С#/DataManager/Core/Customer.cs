using System;

namespace Core
{
    public class Customer
    {
        public string CustomerId { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        public int Age { get; set; }
        public string Occupation { get; set; }
    }
}
