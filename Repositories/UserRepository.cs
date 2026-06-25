    using Microsoft.Data.SqlClient;
    using System.Data;
    using TaskManagementApi.DTOs;
    using TaskManagementApi.Models;

    namespace TaskManagementApi.Repositories;

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            const string sql = @"
                SELECT UserId, UserName, Email
                FROM Users
                ORDER BY UserName";

            using var connection = CreateConnection();
            using var cmd = new SqlCommand(sql, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }
            {


                return users;
            }
        }

        public User? GetUserById(int userId)
        {
            const string sql = @"
                SELECT UserId, UserName, Email
                FROM Users
                WHERE UserId = @UserId";

            using var connection = CreateConnection();
            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            }


            return null;
        }

        public int AddUser(string userName, string email)
        {
            const string sql = @"
                INSERT INTO Users (UserName, Email)
                OUTPUT INSERTED.UserId
                VALUES (@UserName, @Email)";

            using var connection = CreateConnection();
            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = userName;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

            var result = cmd.ExecuteScalar();

            return Convert.ToInt32(result);
        }

        public bool EmailExists(string email)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE Email = @Email";

            using var connection = CreateConnection();
            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public bool UserExists(int userId)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserId = @UserId";

            using var connection = CreateConnection();
            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public UserWithTasksDto? GetUserWithTasks(int userId)
        {
            const string sql = @"
                SELECT
                    u.UserId,
                    u.UserName,
                    u.Email,
                    t.TaskId,
                    t.Title,
                    t.Description,
                    t.Status,
                    t.CreatedDate,
                    t.UserId AS TaskUserId
                FROM Users u
                LEFT JOIN TaskItem t
                    ON u.UserId = t.UserId
                WHERE u.UserId = @UserId
                ORDER BY t.CreatedDate DESC";

            using var connection = CreateConnection();
            using var cmd = new SqlCommand(sql, connection);

            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            using var reader = cmd.ExecuteReader();

            UserWithTasksDto? result = null;

            while (reader.Read())
            {
                if (result == null)
                {
                    result = new UserWithTasksDto
                    {
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        Email = reader.GetString(reader.GetOrdinal("Email"))
                    };
                }

                if (!reader.IsDBNull(reader.GetOrdinal("TaskId")))
                {
                    result.TaskItem.Add(new TaskItemResponseDto
                    {
                        TaskId = reader.GetInt32(reader.GetOrdinal("TaskId")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                        UserId = reader.GetInt32(reader.GetOrdinal("TaskUserId")),
                        UserName = result.UserName
                    });
                }
            }

            return result;
        }

        //public void AddUser()
        //{
        //    throw new NotImplementedException();
        //}

        //List<User> IUserRepository.GetAllUsers()
        //{
        //    throw new NotImplementedException();
        //}

        //User? IUserRepository.GetUserById(int userId)
        //{
        //    throw new NotImplementedException();
        //}

        //UserWithTasksDto? IUserRepository.GetUserWithTasks(int userId)
        //{
        //    throw new NotImplementedException();
        //}
    }