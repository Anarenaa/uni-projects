using Core.Models;

namespace Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository() : base() { }
        
        public void ReplaceAllTags(IEnumerable<Tag> newTags)
        {
            var newTagIds = newTags.Select(t => t.Id).ToList();

            var toDelete = _dbSet
                .Where(dbTag => !newTagIds.Contains(dbTag.Id))
                .ToList();

            if (toDelete.Any())
                _dbSet.RemoveRange(toDelete);

            foreach (var tag in newTags)
            {
                if (tag.Id == 0)
                {
                    if(_dbSet.Any(x => x.Name == tag.Name))
                        continue;
                    _dbSet.Add(tag);
                }
                else
                   _dbSet.Update(tag);
            }
        }
    }
}
