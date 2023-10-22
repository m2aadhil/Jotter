using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using static Jotter.Models.Enum.Enum;

namespace Jotter.Models
{
    public class Audit
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? NoteId { get; set; } = null!;

        public string? UserId { get; set; } = null!;

        public NoteEvent Event { get; set; }
        public DateTime EventAt { get; set; } = DateTime.UtcNow;

    }
}
