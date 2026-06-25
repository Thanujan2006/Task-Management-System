using Task_Management_System.DTOs;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Services;

public interface ITaskService
{
    ApiResponse<List<TaskItemResponseDto>> GetAllTasks();
    ApiResponse<TaskItemResponseDto> GetTaskById(int id);
    ApiResponse<List<TaskItemResponseDto>> SearchTasks(string name);
    //ApiResponse<TaskItemResponseDto> AddTask(CreateTaskItemDto dto);
    ApiResponse<TaskItemResponseDto> UpdateTask(int id, UpdateTaskItemDto dto);
    ApiResponse<TaskItemResponseDto> ChangeStatus(int id, ChangeStatusDto dto);
    ApiResponse<object> DeleteTask(int id);
    object? AddTask(CreateTaskItemDto dto);
    //object? UpdateTask(int id, UpdateTaskItemDto dto);
}
