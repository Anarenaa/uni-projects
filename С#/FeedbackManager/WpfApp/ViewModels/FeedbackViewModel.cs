using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Services;

namespace WpfApp.ViewModels
{
    public partial class FeedbackViewModel : ObservableObject
    {
        private readonly FeedbackService _feedbackService;
        private readonly LogService _logService;
        public FeedbackViewModel(FeedbackService feedbackService, LogService logService)
        {
            _feedbackService = feedbackService;
            _logService = logService;
        }

        [ObservableProperty]
        private string authorName = string.Empty;
        [ObservableProperty]
        private string authorEmail = string.Empty;
        [ObservableProperty]
        private string feedbackRate = string.Empty;
        [ObservableProperty]
        private string feedbackMessage = string.Empty;

        [RelayCommand]
        private void SendFeedback()
        {
            if (!int.TryParse(FeedbackRate, out int rate))
            {
                MessageBox.Show("Введіть ціле число.", 
                    "Invalid Input", MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
                return;
            }

            var feedback = new Feedback
            {
                AuthorName = AuthorName,
                AuthorEmail = AuthorEmail,
                Rate = rate,
                Comment = FeedbackMessage
            };
            
            try
            {
                _feedbackService.AddFeedback(feedback);
                
                var log = new Log
                {
                    Action = Core.Models.Action.Created,
                    EntityType = EntityType.Feedback,
                    EntityId = feedback.Id,
                    AuthorEmail = AuthorEmail
                };
                _logService.AddLog(log);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, 
                    "Error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return;
            }

            AuthorName = string.Empty;
            AuthorEmail = string.Empty;
            FeedbackRate = string.Empty;
            FeedbackMessage = string.Empty;

            var thankYouWindow = new FeedbackThankYouWindow();
            thankYouWindow.ShowDialog();
        }
    }
}
