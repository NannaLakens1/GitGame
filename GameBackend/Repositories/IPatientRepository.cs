using GameBackend.Models;

namespace GameBackend.Repositories
{
    public interface IPatientRepository
    {
        Task InsertAsync(Patient patient);
        Task<Patient?> SelectAsync(Guid id);
        Task<IEnumerable<Patient>> SelectAsyncByUserId(string userId);
        //Task<IEnumerable<Patient>> SelectAsync();
        //Task DeleteAsync(Guid id);
        //Task UpdateAsync(Patient patient);
    }
}