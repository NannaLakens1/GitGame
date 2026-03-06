using Dapper;
using GameBackend.Models;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameBackend.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }
        public async Task<IEnumerable<Object2D>> SelectByEnvironmentAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D] WHERE EnvironmentId = @EnvironmentId", new { EnvironmentId = environmentId });
                //return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D] WHERE Environment2D.Id = @Environment2DId", environmentId);
            }
        }
        public async Task<Object2D?> SelectAsync(Guid id, Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE Id = @Id AND EnvironmentId = @EnvironmentId", new { Id = id, EnvironmentId = environmentId });
            }
        }
        public async Task InsertAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(@"INSERT INTO [Object2D] 
                                                    (Id, EnvironmentId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) 
                                                    VALUES 
                                                    (@Id, @EnvironmentId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)", 
                                                    object2D);
            }
        }
        public async Task UpdateAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    @"UPDATE [Object2D] SET
                    PrefabId = @PrefabId,
                    PositionX = @PositionX,
                    PositionY = @PositionY,
                    ScaleX = @ScaleX,
                    ScaleY = @ScaleY,
                    RotationZ = @RotationZ,
                    SortingLayer = @SortingLayer
                    WHERE Id = @Id
                    AND EnvironmentId = @EnvironmentId",
                    object2D);
            }
        }
        public async Task DeleteAsync(Guid id, Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id AND EnvironmentId = @EnvironmentId", new { Id = id, EnvironmentId = environmentId });
                //await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id }); //waarom new id?
            }
        }
    }
}