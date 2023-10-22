using Jotter.Models;

namespace Jotter.Repository.IRepository
{
    public interface IAuditRepository
    {
        Task<List<Audit>> GetAuditsAsync(string userId);
        Task<List<Audit>> GetAuditsByNoteIdAsync(string userId, string noteId);
        void CreateAudit(Audit audit);
    }
}
