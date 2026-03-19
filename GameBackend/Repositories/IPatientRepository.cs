using GameBackend.Models;

namespace GameBackend.Repositories
{
    public interface IPatientRepository
    {
        Task InsertAsync(Patient patient);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Patient>> SelectAsync();
        Task<Patient?> SelectAsync(Guid id);
        Task<IEnumerable<Patient>> SelectAsyncByUserId(string userId);
        Task UpdateAsync(Patient patient);
    }
}