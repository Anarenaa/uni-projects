using Repositories;
using Core.Models;
using Action = Core.Models.Action;

namespace Services
{
    public class LogService
    {
        private readonly LogRepository _logRepository;
        public LogService(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public List<Log> GetAllLogs()
        {
            return _logRepository.GetAll();
        }
        private void validateLog(Log log)
        {
            if (typeof(Action).IsEnumDefined(log.Action) == false)
                throw new Exception("Action is not defined.");
            if(typeof(EntityType).IsEnumDefined(log.EntityType) == false)
                throw new Exception("EntityType is not defined.");
        }
        public void AddLog(Log log)
        {
            validateLog(log);
            _logRepository.Add(log);
            _logRepository.SaveChanges();
        }
    }
}
