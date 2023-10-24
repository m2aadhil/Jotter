using Jotter.Configs;
using Jotter.Models;
using Jotter.Repository.IRepository;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;

namespace Jotter.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _noteCollection;

        public NoteRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _noteCollection = database.GetCollection<Note>(mongoDBSettings.Value.NotesCollectionName);
        }

        public async Task<List<Note>> GetNotesAsync(string userId)           
        {
            FilterDefinition<Note> filter = Builders<Note>.Filter.And(Builders<Note>.Filter.Eq("Active", true), Builders<Note>.Filter.Eq("UserId", userId));
            SortDefinition<Note> sort = Builders<Note>.Sort.Descending("LastUpdatedAt");
            return await _noteCollection.Find(filter).Sort(sort).ToListAsync();
        }

        private async Task<Note> GetNoteAsync(string id)
        {
            FilterDefinition<Note> filter = Builders<Note>.Filter.And(Builders<Note>.Filter.Where(p=> p.Id == id), Builders<Note>.Filter.Eq("Active", true));
            return await _noteCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateNoteAsync(Note note)
        {
            await _noteCollection.InsertOneAsync(note);
            return;
        }

        public async Task UpdateNoteAsync(string id, Note note)
        {
 
            UpdateDefinition<Note> updatedNote = Builders<Note>.Update.Set("Title", note.Title).Set("Content", note.Content).Set("LastUpdatedAt", DateTime.UtcNow);
            await _noteCollection.UpdateOneAsync(note => note.Id == id, updatedNote);
            return;
        }

        public async Task DeleteNoteAsync(string id) 
        {
            UpdateDefinition<Note> updatedNote = Builders<Note>.Update.Set("Active", false).Set("LastUpdatedAt", DateTime.UtcNow);
            await _noteCollection.UpdateOneAsync(note => note.Id == id, updatedNote);
            return;
        }

        public async Task<Note?> GetNoteIfExists(string id)
        {
            //todo -> add route filter for length
            if (id.Length == 24) {
                return await GetNoteAsync(id);
            }
            return null;
        }
    }
}
