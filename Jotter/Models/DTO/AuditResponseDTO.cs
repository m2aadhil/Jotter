using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using static Jotter.Models.Enum.Enum;

namespace Jotter.Models.DTO
{
    public class AuditResponseDTO
    {

        public string? NoteId { get; set; }
        public string? Event { get; set; }
        public DateTime EventAt { get; set; } 
    }
}
