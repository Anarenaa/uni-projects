using Core.Context;
using Core.Models;

namespace Repositories
{
    public class LogRepository : Repository<Log>
    {
        public LogRepository(DataContext context) : base(context) { }
    }
}
