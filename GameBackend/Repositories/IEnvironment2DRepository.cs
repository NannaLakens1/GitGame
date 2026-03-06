using GameBackend.Models;

namespace GameBackend.Repositories
{
    public interface IEnvironment2DRepository
    {
        Task InsertAsync(Environment2D environment);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Environment2D>> SelectAsync();
        Task<Environment2D?> SelectAsync(Guid id);
        Task<IEnumerable<Environment2D>> SelectAsyncByUserId(string userId);
        Task UpdateAsync(Environment2D environment);
    }
}