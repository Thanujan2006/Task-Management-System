using Microsoft.Data.SqlClient;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    private const string SelectTaskWithUserSql = @"
    SELECT t.TaskId, t.Title, t.Description, t.Status, t.CreatedDate,
           t.UserId, u.UserName
    FROM TaskItem t
    INNER JOIN Users u ON t.UserId = u.UserId";

    public TaskRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public List<TaskItemResponseDto> GetAllTasks()
    {
        var sql = SelectTaskWithUserSql + " ORDER BY t.CreatedDate DESC";
        return ExecuteTaskListQuery(sql);
    }

    public TaskItemResponseDto? GetTaskById(int taskId)
    {
        var sql = SelectTaskWithUserSql + " WHERE t.TaskId = @TaskId";

        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@TaskId", taskId);
        connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapTask(reader);
        }

        return null;
    }

    public List<TaskItemResponseDto> SearchTasks(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return GetAllTasks();
        }

        var sql = SelectTaskWithUserSql + " WHERE t.Title LIKE @Name ORDER BY t.CreatedDate DESC";

        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
        connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        var tasks = new List<TaskItemResponseDto>();
        while (reader.Read())
        {
            tasks.Add(MapTask(reader));
        }

        return tasks;
    }

    public bool UpdateTask(int taskId, string title, string? description, string status, int userId)
    {
        const string sql = @"
            UPDATE TaskItem
            SET Title = @Title, Description = @Description, Status = @Status, UserId = @UserId
            WHERE TaskId = @TaskId";

        using var connection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@TaskId", taskId);
        cmd.Parameters.AddWithValue("@Title", title);
        cmd.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@UserId", userId);
        connection.Open();

        var rows = cmd.ExecuteNonQuery();
        return rows > 0;
    }

    public bool ChangeStatus(int taskId, string status)
    {
        const string sql = "UPDATE TaskItem SET Status = @Status WHERE TaskId = @TaskId";

        using var connection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@TaskId", taskId);
        cmd.Parameters.AddWithValue("@Status", status);
        connection.Open();

        var rows = cmd.ExecuteNonQuery();
        return rows > 0;
    }

    public bool DeleteTask(int taskId)
    {
        const string sql = "DELETE FROM TaskItem WHERE TaskId = @TaskId";

        using var connection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@TaskId", taskId);
        connection.Open();

        var rows = cmd.ExecuteNonQuery();
        return rows > 0;
    }

    public bool TaskExists(int taskId)
    {
        const string sql = "SELECT COUNT(1) FROM TaskItem WHERE TaskId = @TaskId";

        using var connection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@TaskId", taskId);
        connection.Open();

        var count = (int)(cmd.ExecuteScalar() ?? 0);
        return count > 0;
    }

    public List<TaskItemResponseDto> ExecuteTaskListQuery(string sql)
    {

        List<TaskItemResponseDto> tasks = new List<TaskItemResponseDto>();
        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand cmd = new SqlCommand(sql, connection);
        connection.Open();
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            tasks.Add(MapTask(reader));
        }

        return tasks;
    }

    private static TaskItemResponseDto MapTask(SqlDataReader reader)
    {
        return new TaskItemResponseDto
        {
            TaskId = reader.GetInt32(reader.GetOrdinal("TaskId")),
            Title = reader.GetString(reader.GetOrdinal("Title")),
            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                ? null
                : reader.GetString(reader.GetOrdinal("Description")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
            UserName = reader.GetString(reader.GetOrdinal("UserName"))
        };
    }

    public int AddTask(string title, string? description, string status, int userId)
    {
        const string sql = @"
            INSERT INTO TaskItem (Title, Description, Status, UserId)
            OUTPUT INSERTED.TaskId
            VALUES (@Title, @Description, @Status, @UserId)";

        using var connection = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@Title", title);
        cmd.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@UserId", userId);
        connection.Open();

        var result = cmd.ExecuteScalar();
        return Convert.ToInt32(result);
    }
}
