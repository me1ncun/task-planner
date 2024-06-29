using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using taskplanner_scheduler.Database;
using taskplanner_scheduler.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_scheduler.Repositories;

public class TaskRepository
{
    private readonly AppDbContext _context;
    
    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<taskplanner_scheduler.Models.Task>> GetUsersTasks(User user)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            string query = """SELECT tasks.id, tasks.title, tasks.description, tasks.status, tasks.user_id FROM tasks INNER JOIN users ON users.id = tasks.user_id  WHERE users.id = @id;""";

            return await connection.QueryAsync<taskplanner_scheduler.Models.Task>(query, new {user.Id});
        }
    }
}