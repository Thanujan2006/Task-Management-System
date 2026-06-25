using Task_Management_System.DTOs;
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

    public object TaskId { get; private set; }

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public ApiResponse<List<TaskItemResponseDto>> GetAllTasks()
    {

            var tasks = _taskRepository.GetAllTasks();
            return new ApiResponse<List<TaskItemResponseDto>>
            {
                Success = true,
                Message = "Tasks retrieved successfully.",
                Data = tasks
            };

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
        var errors = ValidateTask(dto.Title, dto.Description, dto.Status, dto.TaskId);
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
                dto.TaskId
               );

            var task = _taskRepository.GetTaskById(taskId);

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

    private List<string> ValidateTask(string title, string description, string status, CreateTaskItemDto dto, object taskId)
    {
        throw new NotImplementedException();
    }

    private List<string> ValidateTask(string title, string description, string status)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add("Title is required.");
        else if (title.Trim().Length > 200)
            errors.Add("Title must not exceed 200 characters.");

        if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > 500)
            errors.Add("Description is too long.");

        errors.AddRange(ValidateStatus(status));

        return errors;
    }

    public ApiResponse<TaskItemResponseDto> UpdateTask(int id, UpdateTaskItemDto dto)
    {
        if (id <= 0)
        {
            return ValidationError<TaskItemResponseDto>("Invalid task ID.");
        }

        var errors = ValidateTask(dto.Title, dto.Description, dto.Status, dto.UserId);
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

    private List<string> ValidateTask(string title, string? description, string status, int userId)
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
            errors.Add("Selected user does not exist.");
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

    //ApiResponse<List<TaskItemResponseDto>> ITaskService.GetAllTasks()
    //{
    //    throw new NotImplementedException();
    //}

    //ApiResponse<TaskItemResponseDto> ITaskService.GetTaskById(int id)
    //{
    //    throw new NotImplementedException();
    //}

    //ApiResponse<List<TaskItemResponseDto>> ITaskService.SearchTasks(string name)
    //{
    //    throw new NotImplementedException();
    //}

    //public ApiResponse<TaskItemResponseDto> UpdateTask(int id, UpdateTaskItemDto dto)
    //{
    //    throw new NotImplementedException();
    //}

    //public ApiResponse<TaskItemResponseDto> ChangeStatus(int id, ChangeStatusDto dto)
    //{
    //    throw new NotImplementedException();
    //}

    //ApiResponse<object> ITaskService.DeleteTask(int id)
    //{
    //    throw new NotImplementedException();
    //}

    object? ITaskService.AddTask(CreateTaskItemDto dto)
    {
        return AddTask(dto);
    }




}
