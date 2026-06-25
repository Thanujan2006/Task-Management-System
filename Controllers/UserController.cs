using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var result = _userService.GetAllUsers();
        return result.Success ? Ok(result) : StatusCode(500, result);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        var result = _userService.GetUserById(id);
        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddUser([FromBody] CreateUserDto dto)
    {
        var result = _userService.AddUser(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetUserById), new { id = result.Data!.UserId }, result);
    }

    [HttpGet("{id:int}/tasks")]
    public IActionResult GetUserWithTasks(int id)
    {
        var result = _userService.GetUserWithTasks(id);
        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }
}
