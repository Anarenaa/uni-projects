using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Infrastructure
{
    public interface ITransactionManager
    {
        List<Transaction> Read(string path);
        void Write(string path, List<Transaction> transactions);
    }
}
