using TaskManagementApi.DTOs;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class TaskService : ITaskService
{
    private static readonly HashSet<string> ValidStatuses = new(StringComparer.Ordinal)
    {
        "Todo", "In Progress", "Done"
    };

    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public ApiResponse<List<TaskItemResponseDto>> GetAllTasks()
    {
        try
        {
            var tasks = _taskRepository.GetAllTasks();
            return new ApiResponse<List<TaskItemResponseDto>>
            {
                Success = true,
                Message = "Tasks retrieved successfully.",
                Data = tasks
            };
        }
        catch
        {
            return ErrorResponse<List<TaskItemResponseDto>>("An error occurred while retrieving tasks.");
        }
    }

    public ApiResponse<TaskItemResponseDto> GetTaskById(int id)
    {
        if (id <= 0)
        {
            return ValidationError<TaskItemResponseDto>("Invalid task ID.");
        }

        try
        {
            var task = _taskRepository.GetTaskById(id);
            if (task == null)
            {
                return NotFound<TaskItemResponseDto>("Task not found.");
            }

            return new ApiResponse<TaskItemResponseDto>
            {
                Success = true,
                Message = "Task retrieved successfully.",
                Data = task
            };
        }
        catch
        {
            return ErrorResponse<TaskItemResponseDto>("An error occurred while retrieving the task.");
        }
    }

    public ApiResponse<List<TaskItemResponseDto>> SearchTasks(string name)
    {
        try
        {
            var tasks = _taskRepository.SearchTasks(name);
            return new ApiResponse<List<TaskItemResponseDto>>
            {
                Success = true,
                Message = "Tasks retrieved successfully.",
                Data = tasks
            };
        }
        catch
        {
            return ErrorResponse<List<TaskItemResponseDto>>("An error occurred while searching tasks.");
        }
    }

    public ApiResponse<TaskItemResponseDto> AddTask(CreateTaskItemDto dto)
    {
        var errors = ValidateCreateTask(dto);
        if (errors.Count > 0)
        {
            return new ApiResponse<TaskItemResponseDto>
            {
                Success = false,
                Message = "Validation failed.",
                Errors = errors
            };
        }

        try
        {
            var taskId = _taskRepository.AddTask(
                dto.Title.Trim(),
                dto.Description?.Trim(),
                dto.Status.Trim(),
                dto.UserId);

            var task = _taskRepository.GetTaskById(taskId);
            if (task == null)
            {
                return ErrorResponse<TaskItemResponseDto>("An error occurred while creating the task.");
            }

            return new ApiResponse<TaskItemResponseDto>
            {
                Success = true,
                Message = "Task created successfully.",
                Data = task
            };
        }
        catch
        {
            return ErrorResponse<TaskItemResponseDto>("An error occurred while creating the task.");
        }
    }

    public ApiResponse<TaskItemResponseDto> UpdateTask(int id, UpdateTaskItemDto dto)
    {
        if (id <= 0)
        {
            return ValidationError<TaskItemResponseDto>("Invalid task ID.");
        }

        var errors = ValidateTaskInput(dto.Title, dto.Description, dto.Status, dto.UserId);
        if (errors.Count > 0)
        {
            return new ApiResponse<TaskItemResponseDto>
            {
                Success = false,
                Message = "Validation failed.",
                Errors = errors
            };
        }

        try
        {
            if (!_taskRepository.TaskExists(id))
            {
                return NotFound<TaskItemResponseDto>("Task not found.");
            }

            _taskRepository.UpdateTask(
                id,
                dto.Title.Trim(),
                dto.Description?.Trim(),
                dto.Status.Trim(),
                dto.UserId);

            var task = _taskRepository.GetTaskById(id);

            return new ApiResponse<TaskItemResponseDto>
            {
                Success = true,
                Message = "Task updated successfully.",
                Data = task
            };
        }
        catch
        {
            return ErrorResponse<TaskItemResponseDto>("An error occurred while updating the task.");
        }
    }

    public ApiResponse<TaskItemResponseDto> ChangeStatus(int id, ChangeStatusDto dto)
    {
        if (id <= 0)
        {
            return ValidationError<TaskItemResponseDto>("Invalid task ID.");
        }

        var errors = ValidateStatus(dto.Status);
        if (errors.Count > 0)
        {
            return new ApiResponse<TaskItemResponseDto>
            {
                Success = false,
                Message = "Validation failed.",
                Errors = errors
            };
        }

        try
        {
            if (!_taskRepository.TaskExists(id))
            {
                return NotFound<TaskItemResponseDto>("Task not found.");
            }

            _taskRepository.ChangeStatus(id, dto.Status.Trim());
            var task = _taskRepository.GetTaskById(id);

            return new ApiResponse<TaskItemResponseDto>
            {
                Success = true,
                Message = "Task status updated successfully.",
                Data = task
            };
        }
        catch
        {
            return ErrorResponse<TaskItemResponseDto>("An error occurred while updating task status.");
        }
    }

    public ApiResponse<object> DeleteTask(int id)
    {
        if (id <= 0)
        {
            return ValidationError<object>("Invalid task ID.");
        }

        try
        {
            if (!_taskRepository.TaskExists(id))
            {
                return NotFound<object>("Task not found.");
            }

            _taskRepository.DeleteTask(id);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Task deleted successfully."
            };
        }
        catch
        {
            return ErrorResponse<object>("An error occurred while deleting the task.");
        }
    }

    private List<string> ValidateCreateTask(CreateTaskItemDto dto)
    {
        return ValidateTaskInput(dto.Title, dto.Description, dto.Status, dto.UserId);
    }

    private List<string> ValidateTaskInput(string title, string? description, string status, int userId)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add("Title is required.");
        }
        else if (title.Trim().Length > 200)
        {
            errors.Add("Title must not exceed 200 characters.");
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > 500)
        {
            errors.Add("Description is too long.");
        }

        errors.AddRange(ValidateStatus(status));

        if (userId <= 0)
        {
            errors.Add("UserId is required. Use GET /api/Users to get a valid user ID.");
        }
        else if (!_userRepository.UserExists(userId))
        {
            errors.Add("Selected user does not exist.");
        }

        return errors;
    }

    private static List<string> ValidateStatus(string status)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(status))
        {
            errors.Add("Invalid status.");
        }
        else if (!ValidStatuses.Contains(status.Trim()))
        {
            errors.Add("Status must be Todo, In Progress, or Done.");
        }

        return errors;
    }

    private static ApiResponse<T> ValidationError<T>(string message) => new()
    {
        Success = false,
        Message = "Validation failed.",
        Errors = new List<string> { message }
    };

    private static ApiResponse<T> NotFound<T>(string message) => new()
    {
        Success = false,
        Message = message
    };

    private static ApiResponse<T> ErrorResponse<T>(string message) => new()
    {
        Success = false,
        Message = message
    };
}
