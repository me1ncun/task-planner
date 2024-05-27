using Dapper;
using Npgsql;
using taskplanner_scheduler.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_scheduler.Repositories;

public class TaskRepository
{
    private readonly IConfiguration _configuration;
    private readonly string sqlString;
    
    public TaskRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        sqlString = _configuration.GetConnectionString("Database");
    }
    
    public async Task<IEnumerable<taskplanner_scheduler.Models.Task>> GetUsersTask(User user)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(sqlString))
        {
            string query = """SELECT tasks.id, tasks.title, tasks.description, tasks.status, tasks.user_id FROM tasks INNER JOIN users ON users.id = tasks.user_id  WHERE users.id = @id;""";

            return await connection.QueryAsync<taskplanner_scheduler.Models.Task>(query, new {user.Id});
        }
    }
}