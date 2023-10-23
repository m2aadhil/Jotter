using Jotter.Models;

namespace Jotter.Repository.IRepository
{
    public interface INoteRepository
    {
        Task<List<Note>> GetNotesAsync(string userId);
        Task CreateNoteAsync(Note note);
        Task DeleteNoteAsync(string id);
        Task UpdateNoteAsync(string id, Note note);
        Task<Note?> GetNoteIfExists(string id);
    }
}
