using WpfApp.ViewModels;
using WpfApp.Models;

namespace WpfApp.Messages
{
    public class BugAddedMessage
    {
        public BugItem NewBug { get; }
        public BugAddedMessage(BugItem bug) => NewBug = bug;
    }
}
