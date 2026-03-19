using GameBackend.Models;

namespace GameBackend.Repositories
{
    public interface IBehandelingRepository
    {
        Task<IEnumerable<Behandeling>> SelectByPatientAsync(Guid patientId);
        Task<Behandeling?> SelectAsync(Guid id, Guid patientId);
        Task InsertAsync(Behandeling behandeling);
        Task UpdateAsync(Behandeling behandeling);
        Task DeleteAsync(Guid id, Guid patientId);
    }
}
