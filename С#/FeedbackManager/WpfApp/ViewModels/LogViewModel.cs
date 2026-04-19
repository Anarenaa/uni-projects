using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.Models;
using Services;

namespace WpfApp.ViewModels
{
    public partial class LogViewModel : ObservableObject
    {
        private readonly LogService _logService;
        [ObservableProperty]
        public string _userString;

        [ObservableProperty]
        private ObservableCollection<Log> _logs;

        public LogViewModel(LogService logService)
        {
            _logService = logService;

            loadLogs();
        }

        private void loadLogs()
        {
            var logs = _logService.GetAllLogs().OrderByDescending(x => x.Timestamp);
            Logs = new ObservableCollection<Log>(logs);
        }
    }
}
