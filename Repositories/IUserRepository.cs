using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface IUserRepository
    {

        public List<User> GetAllUsers();

        public User? GetUserById(int UserId);
        public void AddUser();
        public User? GetUserWithTasks(int UserId);






    }
}
