using Jotter.Models;

namespace Jotter.Repository.IRepository
{
    public interface IAuditRepository
    {
        Task<List<Audit>> GetAuditsAsync();
        void CreateAudit(Audit audit);
    }
}
