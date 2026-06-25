using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public interface IUserRepository
{
    List<User> GetAllUsers();
    User? GetUserById(int userId);
    int AddUser(string userName, string email);
    bool EmailExists(string email);
    bool UserExists(int userId);
    UserWithTasksDto? GetUserWithTasks(int userId);

}
