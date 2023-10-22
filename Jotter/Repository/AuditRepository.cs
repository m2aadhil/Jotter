using Jotter.Configs;
using Jotter.Models;
using Jotter.Repository.IRepository;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Jotter.Repository
{
    public class AuditRepository: IAuditRepository
    {
        private readonly IMongoCollection<Audit> _auditCollection;

        public AuditRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _auditCollection = database.GetCollection<Audit>(mongoDBSettings.Value.AuditCollectionName);
        }

        public async Task<List<Audit>> GetAuditsAsync(string userId)           
        {
            return await _auditCollection.Find(audit => audit.UserId == userId).ToListAsync();
        }

        public async Task<List<Audit>> GetAuditsByNoteIdAsync(string userId, string noteId)
        {
            return await _auditCollection.Find(audit => audit.UserId == userId &&  audit.NoteId == noteId ).ToListAsync();
        }

        public void CreateAudit(Audit audit)
        {
            _auditCollection.InsertOne(audit);
        }
    }
}
