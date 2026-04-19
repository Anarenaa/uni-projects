using System.ComponentModel.DataAnnotations;
using Core.Models;
using Repositories;

namespace Services
{
    public class FeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;
        private readonly FeedbackCategoryRepository _fcRepository;
        public FeedbackService(FeedbackRepository feedbackRepository, FeedbackCategoryRepository fcRep)
        {
            _feedbackRepository = feedbackRepository;
            _fcRepository = fcRep;
        }
        public List<Feedback> GetAllFeedbacks()
        {
            return _feedbackRepository.GetAll();
        }
        public IEnumerable<Feedback> GetFilteredFeedbacks(bool isAnalyzed, IEnumerable<int>? categoryIds = null, string? searchText = null)
        {
            return _feedbackRepository.GetAllFiltered(isAnalyzed, categoryIds, searchText);
        }
        public Feedback? GetFeedbackById(int id)
        {
            var feedback = _feedbackRepository.GetById(id);
            if (feedback == null)
                throw new Exception("Feedback is not found");
            return feedback;
        }
        private void validateFeedback(Feedback feedback)
        {
            if (string.IsNullOrWhiteSpace(feedback.AuthorName))
                throw new Exception("Author name cannot be empty.");
            if (feedback.AuthorName.Length > 50)
                throw new Exception("Author name is too long.");
            if (string.IsNullOrWhiteSpace(feedback.AuthorEmail))
                throw new Exception("Invalid author email.");
            if (feedback.AuthorEmail.Length > 100)
                throw new Exception("Author email is too long.");
            if (feedback.Rate < 0 || feedback.Rate > 10)
                throw new Exception("Rate must be between 0 and 10.");
            if (string.IsNullOrWhiteSpace(feedback.Comment))
                throw new Exception("Feedback content cannot be empty.");
            if (!new EmailAddressAttribute().IsValid(feedback.AuthorEmail))
                throw new Exception("Invalid author email.");

        }
        public void AddFeedback(Feedback feedback)
        {
            validateFeedback(feedback);
            _feedbackRepository.Add(feedback);
            _feedbackRepository.SaveChanges();
        }
        public void AnalyzeFeedback(Feedback feedback, bool IsImportant, User currentUser)
        {
            var existingFeedback = _feedbackRepository.GetById(feedback.Id);
            if (existingFeedback == null)
                throw new Exception("Feedback is not found");

            existingFeedback.IsAnalyzed = true;
            existingFeedback.IsImportant = IsImportant;
            existingFeedback.AnalystId = currentUser.Id;
            existingFeedback.AnalyzedAt = DateTime.Now;
            existingFeedback.LastUpdatedAt = DateTime.Now;

            existingFeedback.FeedbackCategories.Clear();

            foreach (var tempTag in feedback.TempTags)
            {
                existingFeedback.FeedbackCategories.Add(new FeedbackCategory
                {
                    FeedbackId = existingFeedback.Id,
                    CategoryId = tempTag.Id
                });
            }
            _fcRepository.SaveChanges();
            _feedbackRepository.SaveChanges();
        }
    }
}
