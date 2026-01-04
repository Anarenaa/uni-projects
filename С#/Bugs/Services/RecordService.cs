using Core.Models;
using Repositories;

namespace Services
{
    public class RecordService
    {
        private readonly RecordRepository _recordRepository;
        private readonly RecordTagRepository _recordTagRepository;
        public RecordService()
        {
            _recordRepository = new RecordRepository();
            _recordTagRepository = new RecordTagRepository();
        }
        private void validateRecord(Record record)
        {
            if(string.IsNullOrEmpty(record.Error))
                throw new Exception("Error field is required.");
            if(string.IsNullOrEmpty(record.Solution))
                throw new Exception("Solution field is required.");
        }
        
        public List<Record> GetAllRecords()
        {
            return _recordRepository.Get().ToList();
        }
        public void AddRecord(Record record)
        {
            validateRecord(record);
            _recordRepository.Add(record);
            _recordRepository.SaveChanges();
        }
        public void EditRecord(int id, Record record)
        {
            validateRecord(record);
            var existingRecord = _recordRepository.GetById(id);
            if(existingRecord == null)
                throw new Exception("Record is not found.");

            existingRecord.Error = record.Error;
            existingRecord.Context = record.Context;
            existingRecord.Solution = record.Solution;
            _recordRepository.SaveChanges();
        }
        public void DeleteRecord(int recordId)
        {
            var record = _recordRepository.GetById(recordId);
            if (record == null)
                throw new Exception("Record is not found.");

            _recordRepository.Delete(record);
            _recordRepository.SaveChanges();
        }

        public List<Tag> GetTagsByRecordId(int recordId)
        {
            return _recordTagRepository.Get()
                .Where(rt => rt.RecordId == recordId)
                .Select(rt => rt.Tag)
                .ToList();
        }
        public void AddTagToRecord(int recordId, int tagId)
        {
            _recordTagRepository.Add(new RecordTag { RecordId = recordId, TagId = tagId });
            _recordTagRepository.SaveChanges();
        }
        public void RemoveTagFromRecord(int recordId, int tagId)
        {
            var recordTag = _recordTagRepository.Get()
                .FirstOrDefault(rt => rt.RecordId == recordId && rt.TagId == tagId);
            if (recordTag != null)
            {
                _recordTagRepository.Delete(recordTag);
                _recordTagRepository.SaveChanges();
            }
        }
    }
}
