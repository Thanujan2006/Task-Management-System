using System.Net.Mail;
using System.Text.RegularExpressions;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public ApiResponse<List<User>> GetAllUsers()
    {
        try
        {
            var users = _userRepository.GetAllUsers();
            return new ApiResponse<List<User>>
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = users
            };
        }
        catch
        {
            return ErrorResponse<List<User>>("An error occurred while retrieving users.");
        }
    }

    public ApiResponse<User> GetUserById(int id)
    {
        if (id <= 0)
        {
            return ValidationError<User>("Invalid user ID.");
        }

        try
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound<User>("User not found.");
            }

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = user
            };
        }
        catch
        {
            return ErrorResponse<User>("An error occurred while retrieving the user.");
        }
    }

    public ApiResponse<User> AddUser(CreateUserDto dto)
    {
        var errors = ValidateUser(dto);
        if (errors.Count > 0)
        {
            return new ApiResponse<User>
            {
                Success = false,
                Message = "Validation failed.",
                Errors = errors
            };
        }

        try
        {
            if (_userRepository.EmailExists(dto.Email.Trim()))
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = new List<string> { "Valid unique email is required." }
                };
            }

            var userId = _userRepository.AddUser(dto.UserName.Trim(), dto.Email.Trim());
            var user = _userRepository.GetUserById(userId);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User created successfully.",
                Data = user
            };
        }
        catch
        {
            return ErrorResponse<User>("An error occurred while creating the user.");
        }
    }

    public ApiResponse<UserWithTasksDto> GetUserWithTasks(int UserId)
    {
        if (UserId <= 0)
        {
            return ValidationError<UserWithTasksDto>("Invalid user ID.");
        }

        try
        {
            var userWithTasks = _userRepository.GetUserWithTasks(UserId);
            if (userWithTasks == null)
            {
                return NotFound<UserWithTasksDto>("User not found.");
            }

            return new ApiResponse<UserWithTasksDto>
            {
                Success = true,
                Message = "User with tasks retrieved successfully.",
                Data = userWithTasks
            };
        }
        catch
        {
            return ErrorResponse<UserWithTasksDto>("An error occurred while retrieving user tasks.");
        }
    }

    private static List<string> ValidateUser(CreateUserDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.UserName))
        {
            errors.Add("UserName is required.");
        }
        else if (dto.UserName.Trim().Length > 100)
        {
            errors.Add("UserName must not exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            errors.Add("Valid unique email is required.");
        }
        else if (dto.Email.Trim().Length > 100)
        {
            errors.Add("Email must not exceed 100 characters.");
        }
        else if (!IsValidEmail(dto.Email.Trim()))
        {
            errors.Add("Valid unique email is required.");
        }

        return errors;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        catch
        {
            return false;
        }
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
