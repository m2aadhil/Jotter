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

        public async Task<List<Audit>> GetAuditsAsync()           
        {
            return await _auditCollection.Find(_ => true).ToListAsync();
        }

        public void CreateAudit(Audit audit)
        {
            _auditCollection.InsertOne(audit);
        }
    }
}
