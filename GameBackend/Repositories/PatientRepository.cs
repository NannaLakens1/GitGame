using GameBackend.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace GameBackend.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string sqlConnectionString;

        public PatientRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Patient patient)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("INSERT INTO [Patient] (Id, Name, Age, UserId) VALUES (@Id, @Name, @Age, @UserId)", patient);
            }
        }

        //een patient ophalen met de userId van de ouder (=authenticated user)
        public async Task<IEnumerable<Patient>> SelectAsyncByUserId(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Patient>("SELECT * FROM [Patient] WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        //een patient ophalen met de Id van de patient zelf
        public async Task<Patient?> SelectAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Patient>("SELECT * FROM [Patient] WHERE Id = @Id", new { id });
            }
        }

        /*public async Task<IEnumerable<Patient>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Patient>("SELECT * FROM [Patient]");
            }
        }
        public async Task UpdateAsync(Patient patient)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Patient] SET " +
                                                 "Name = @Name " +
                                                 "WHERE Id = @Id", patient);

            }
        }
        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Patient] WHERE Id = @Id", new { id });
            }
        }*/

    }
}