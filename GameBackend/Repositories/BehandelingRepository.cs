using Dapper;
using GameBackend.Models;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameBackend.Repositories
{
    public class BehandelingRepository : IBehandelingRepository
    {
        private readonly string sqlConnectionString;

        public BehandelingRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }
        public async Task<IEnumerable<Behandeling>> SelectByPatientAsync(Guid patientId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Behandeling>("SELECT * FROM [Behandeling] WHERE PatientId = @PatientId", new { PatientId = patientId });
            }
        }
        public async Task<Behandeling?> SelectAsync(Guid id, Guid patientId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Behandeling>("SELECT * FROM [Behandeling] WHERE Id = @Id AND PatientId = @PatientId", new { Id = id, PatientId = patientId });
            }
        }
        public async Task InsertAsync(Behandeling behandeling)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(@"INSERT INTO [Object2D] 
                                                    (Id, EnvironmentId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) 
                                                    VALUES 
                                                    (@Id, @EnvironmentId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)", 
                                                    behandeling);
            }
        }
        public async Task UpdateAsync(Behandeling behandeling)
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
                    behandeling);
            }
        }
        public async Task DeleteAsync(Guid id, Guid patientId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Behandeling] WHERE Id = @Id AND PatientId = @PatientId", new { Id = id, PatientId = patientId });
                //await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id }); //waarom new id?
            }
        }
    }
}