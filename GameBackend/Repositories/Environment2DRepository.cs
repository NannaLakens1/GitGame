using GameBackend.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace GameBackend.Repositories
{
    public class Environment2DRepository : IEnvironment2DRepository
    {
        private readonly string sqlConnectionString;

        public Environment2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        //dit is nu zonder max height en length, ze mogen ook null zijn, maar moet ik hier aparte voor maken voor als dat wel erin wordt gezet?
        public async Task InsertAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("INSERT INTO [Environment2D] (Id, Name, UserId) VALUES (@Id, @Name, @UserId)", environment);
            }
        }

        //delete from environment of delete environment
        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Environment2D] WHERE Id = @Id", new { id });
            }
        }
        //alle environments ophalen
        public async Task<IEnumerable<Environment2D>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D]");
            }
        }

        public async Task<IEnumerable<Environment2D>> SelectAsyncByUserId(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        //alles van 1 environment ophalen, dus voor als je een wereld wilt openen?
        public async Task<Environment2D?> SelectAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { id });
            }
        }

        // met of zonder max height en length? of moeten dat lossen, of iets met put of patch?
        // eerst was het met:"Number = @Number " + (waarom?)
        public async Task UpdateAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Environment2D] SET " +
                                                 "Name = @Name " +
                                                 "WHERE Id = @Id", environment);

            }
        }

    }
}
/*Een methode die de wereld opslaan met de nieuwe/veranderde objecten erin, dan hoef je de objecten niet te koppelen aan sql perse, of moet het meteen
 * opgeslagen worden als het in de wereld wordt gezet?
 */
