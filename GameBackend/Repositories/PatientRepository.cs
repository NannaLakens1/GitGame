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

        //dit is nu zonder max height en length, ze mogen ook null zijn, maar moet ik hier aparte voor maken voor als dat wel erin wordt gezet?
        public async Task InsertAsync(Patient patient)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("INSERT INTO [Patient] (Id, Name, UserId) VALUES (@Id, @Name, @UserId)", environment);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Patient] WHERE Id = @Id", new { id });
            }
        }
        //alle environments ophalen
        public async Task<IEnumerable<Patient>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Patient>("SELECT * FROM [Patient]");
            }
        }

        public async Task<IEnumerable<Patient>> SelectAsyncByUserId(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Patient>("SELECT * FROM [Patient] WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        public async Task<Patient?> SelectAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Patient>("SELECT * FROM [Patient] WHERE Id = @Id", new { id });
            }
        }

        // met of zonder max height en length? of moeten dat lossen, of iets met put of patch?
        // eerst was het met:"Number = @Number " + (waarom?)
        public async Task UpdateAsync(Patient patient)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Patient] SET " +
                                                 "Name = @Name " +
                                                 "WHERE Id = @Id", patient);

            }
        }

    }
}