using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public class UserRepository : IUserRepository

    {

        private readonly string _connectionstring;

        public object Email { get; private set; }
        public object Name { get; private set; }
        public object UserId { get; private set; }

        public UserRepository(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("default");
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            SqlConnection connect = new SqlConnection(_connectionstring);
            connect.Open();
            string Quary = "SELECT * FROM Users";
            SqlCommand command = new SqlCommand(Quary, connect);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User()
                {

                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["Name"],
                    Email = (string)reader["Email"]



                };
                users.Add(user);


            }
            return users;
        }



        public User? GetUserById(int UserId)
        {
            User user1 = null;
            SqlConnection connect = new SqlConnection(_connectionstring);
            connect.Open();
            string Quary = "SELECT UserId,UserName,Email FROM Users WHERE UserId=@UserId";
            SqlCommand command = new SqlCommand(Quary, connect);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User()
                {

                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["Name"],
                    Email = (string)reader["Email"]



                };
            }
            return user1;
        }


        public void AddUser()
        {

            using SqlConnection connect = new SqlConnection(_connectionstring);
            connect.Open();

            string query = @"INSERT INTO Users
                     (UserId, Name, Email)
                     VALUES
                     (@UserId, @Name, @Email)";

            SqlCommand command = new SqlCommand(query, connect);

            command.Parameters.AddWithValue("@UserId", UserId);
            command.Parameters.AddWithValue(" @Name", Name);
            command.Parameters.AddWithValue(" @Email", Email);
            command.ExecuteNonQuery();

        }

        public User? GetUserWithTasks(int UserId)
        {

            User user1 = null;
            SqlConnection connect = new SqlConnection(_connectionstring);
            connect.Open();
            string Quary = @"SELECT UserId,UserName,Email FROM Users WHERE INNER JOIN TaskItem" +
                "ON User.UserId=TaskItem.UserId";
            SqlCommand command = new SqlCommand(Quary, connect);
            command.Parameters.AddWithValue("@UserId", UserId);
            command.Parameters.AddWithValue(" @Name", Name);
            command.Parameters.AddWithValue(" @Email", Email);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User user = new User()
                {

                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["Name"],
                    Email = (string)reader["Email"]



                };
            }
            return user1;
        }

    }













}













