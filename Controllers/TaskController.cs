using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var result = _taskService.GetAllTasks();
        return result.Success ? Ok(result) : StatusCode(500, result);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetTaskById(int id)
    {
        var result = _taskService.GetTaskById(id);
        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("search")]
    public IActionResult SearchTasks([FromQuery] string? name)
    {
        var result = _taskService.SearchTasks(name ?? string.Empty);
        return result.Success ? Ok(result) : StatusCode(500, result);
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] CreateTaskItemDto dto)
    {
        var result = _taskService.AddTask(dto);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetTaskById), new { id = result.Data!.TaskId }, result);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateTask(int id, [FromBody] UpdateTaskItemDto dto)
    {
        var result = _taskService.UpdateTask(id, dto);
        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:int}/status")]
    public IActionResult ChangeStatus(int id, [FromBody] ChangeStatusDto dto)
    {
        var result = _taskService.ChangeStatus(id, dto);
        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteTask(int id)
    {
        var result = _taskService.DeleteTask(id);
        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }
}
