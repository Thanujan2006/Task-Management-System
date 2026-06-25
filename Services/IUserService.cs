using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public interface IUserService
{
    ApiResponse<List<User>> GetAllUsers();
    ApiResponse<User> GetUserById(int id);
    ApiResponse<User> AddUser(CreateUserDto dto);
    ApiResponse<UserWithTasksDto> GetUserWithTasks(int id);
}
