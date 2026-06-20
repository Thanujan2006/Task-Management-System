using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public class UserRepository
    {

        private readonly string _connectionstring;

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

        }













    }









        
    }


