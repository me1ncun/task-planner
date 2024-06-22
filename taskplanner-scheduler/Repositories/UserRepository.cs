using Dapper;
using Npgsql;
using taskplanner_scheduler.Models;

namespace taskplanner_scheduler.Repositories;

public class UserRepository
{
    private readonly IConfiguration _configuration;
    private readonly string sqlString;
    
    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        sqlString = _configuration.GetConnectionString("Database");
    }
    
    public async Task<IEnumerable<User>> GetAll()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(sqlString))
        {
            string query = """SELECT * FROM users;""";

            return await connection.QueryAsync<User>(query);
        }
    }
}