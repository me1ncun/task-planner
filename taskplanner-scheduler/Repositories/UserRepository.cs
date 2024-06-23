using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using taskplanner_scheduler.Database;
using taskplanner_scheduler.Models;

namespace taskplanner_scheduler.Repositories;

public class UserRepository
{
    private readonly IConfiguration _configuration;
    private readonly string connectionString;
    
    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("Database");
    }
    
    public async Task<IEnumerable<User>> GetAll()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            string query = """SELECT * FROM users;""";

            return await connection.QueryAsync<User>(query);
        }
    }
}