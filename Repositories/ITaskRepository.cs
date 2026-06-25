using TaskManagementApi.DTOs;

namespace TaskManagementApi.Repositories;

public interface ITaskRepository
{
    List<TaskItemResponseDto> GetAllTasks();
    TaskItemResponseDto? GetTaskById(int taskId);
    List<TaskItemResponseDto> SearchTasks(string name);
    int AddTask(string title, string? description, string status, int userId);
    bool UpdateTask(int taskId, string title, string? description, string status, int userId);
    bool ChangeStatus(int taskId, string status);
    bool DeleteTask(int taskId);
    bool TaskExists(int taskId);
    int AddTask(string v1, string? v2, string v3);
}
