namespace Jotter.Configs
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string NotesCollectionName { get; set; } = null!;
        public string AuditCollectionName { get; set; } = null!;
    }
}
