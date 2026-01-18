using Core.Models;
using Repositories;

namespace Services
{
    public class TagService
    {
        private readonly TagRepository _tagRepository;
        public TagService()
        {
            _tagRepository = new TagRepository();
        }
        private void validateTag(Tag tag)
        {
            if(string.IsNullOrEmpty(tag.Name))
                throw new Exception("Tag name is required.");
            if (tag.Name.Length > 50)
                throw new Exception("Tag name is too long.");
        }
        public List<Tag> GetAllTags()
        {
            return _tagRepository.Get().ToList();
        }
        public void AddTag(Tag tag)
        {
            validateTag(tag);

            _tagRepository.Add(tag);
            _tagRepository.SaveChanges();
        }
        public void EditTag(Tag tag)
        {
            validateTag(tag);
            var existingTag = _tagRepository.GetById(tag.Id.Value);
            if(existingTag == null)
                throw new Exception("Tag is not found.");

            existingTag.Name = tag.Name;
            _tagRepository.SaveChanges();
        }
        public void DeleteTag(Tag tag)
        {
            _tagRepository.Delete(tag);
            _tagRepository.SaveChanges();
        }
        public void ReplaceAllTags(IEnumerable<Tag> newTags)
        {
            foreach (var tag in newTags)
            {
                validateTag(tag);
            }
            _tagRepository.ReplaceAllTags(newTags);
            _tagRepository.SaveChanges();
        }
    }
}
