using Microsoft.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Task_Management_System.Models;
using Task_Management_System.NewFolder;

namespace Task_Management_System.Repositories
{
    public class TaskRepository : ITaskRepository

    {
        private readonly string _connectionString;
        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }


        public List<TaskItem> GetAllTasks()
        {
            List<TaskItem> task1 = new List<TaskItem>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = "SELECT * FROM TaskItem";
            SqlCommand cmd = new SqlCommand(quary, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                TaskItem item = new TaskItem()
                {
                    TaskId = (int)reader["TaskId"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    Status = (string)reader["Status"],
                };
                task1.Add(item);



            }
            return task1;
        }
        public TaskItem GetTaskById(int TaskId)
        {
            TaskItem task1 = null;
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = "SELECT * FROM TaskItem WHERE TaskId=@TaskId ";
            SqlCommand cmd = new SqlCommand(quary, conn);
            cmd.Parameters.AddWithValue("@TaskId", TaskId);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                TaskItem item = new TaskItem()
                {
                    TaskId = (int)reader["TaskId"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    Status = (string)reader["Status"],
                };




            }
            return task1;
        }


        public TaskItem SearchTasks(string Title)
        {
            TaskItem task1 = null;
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = "SELECT Title FROM TaskItem WHERE Title LIKE 'L%' OR Title LIKE 'U%'OR Title LIKE 'C%' OR Title LIKE 'B%' ";
            SqlCommand cmd = new SqlCommand(quary, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                TaskItem item = new TaskItem()
                {

                    Title = (string)reader["Title"]

                };

            }
            return task1;

        }

        public List<TaskItem> AddTask(int TaskId, string Title,string Description, string Status,int UserId)
        {
            List<TaskItem> task1 = new List<TaskItem>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = "INSERT INTO (TaskId,Title,Description,Status,UserId)VALUES( @TaskId,@Title,@Description,@Status,@UserId)";



            SqlCommand cmd = new SqlCommand(quary, conn);
            cmd.Parameters.AddWithValue("@TaskId", TaskId);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                TaskItem item = new TaskItem()
                {
                    TaskId = (int)reader["TaskId"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    Status = (string)reader["Status"],
                };
                task1.Add(item);

            }
            return task1;



        }
        public List<TaskItem> UpdateTask(int TaskId, string Title, string Description, string Status, int UserId)
        {
            List<TaskItem> task1 = new List<TaskItem>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = "UPDATE SET TaskId=@TaskId,Title=@Title,Description=@Description,Status=@Status,UserId=@UserId FROM TaskItem";



            SqlCommand cmd = new SqlCommand(quary, conn);
            cmd.Parameters.AddWithValue("@TaskId", TaskId);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                TaskItem item = new TaskItem()
                {
                    TaskId = (int)reader["TaskId"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    Status = (string)reader["Status"],
                };
                task1.Add(item);

            }
            return task1;




        }


        public bool ChangeStatus(string Status ,int TaskId)
        {
            TaskItem task1 = null;
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = "UPDATE SET Status=@Status FROM TaskItem WHERE TaskId=@TaskId";
            SqlCommand cmd = new SqlCommand(quary, conn);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@TaskId", TaskId);
            cmd.ExecuteNonQuery();
            return true;
          
        }



        public void DeleteTask(string Status,int TaskId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            string quary = " DELETE Status=@Status FROM TaskItem WHERE TaskId=@TaskId";
            SqlCommand cmd = new SqlCommand(quary, conn);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@TaskId", TaskId);
            cmd.ExecuteNonQuery();



        }

        TaskItem ITaskRepository.ChangeStatus(string Status, int TaskId)
        {
            throw new NotImplementedException();
        }
    }
}
