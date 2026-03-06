using GameBackend.Models;

namespace GameBackend.Repositories
{
    public interface IObject2DRepository
    {
        Task<IEnumerable<Object2D>> SelectByEnvironmentAsync(Guid environmentId);
        Task<Object2D?> SelectAsync(Guid id, Guid environmentId);
        Task InsertAsync(Object2D object2D);
        Task UpdateAsync(Object2D object2D);
        Task DeleteAsync(Guid id, Guid environmentId);
    }
}
