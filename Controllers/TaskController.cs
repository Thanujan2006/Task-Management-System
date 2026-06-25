using Microsoft.AspNetCore.Mvc;
using Task_Management_System.DTOs;
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
        var result = _taskService.GetAllTasks() as dynamic; // Temporary fix if you don't know the type
        if (result == null)
            return StatusCode(500, "Unexpected result type from service.");

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
        var result = _taskService.SearchTasks(name ?? string.Empty) as dynamic; // Temporary fix
        if (result == null)
            return StatusCode(500, "Unexpected result type from service.");

        return result.Success ? Ok(result) : StatusCode(500, result);
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] CreateTaskItemDto dto)
    {
        var result = _taskService.AddTask(dto);
        if (result == null)
            return StatusCode(500, "Unexpected result type from service.");

        dynamic dynamicResult = result;
        if (dynamicResult.Success)
        {
            // Assuming the result contains the created task with its ID
            return CreatedAtAction(nameof(GetTaskById), new { id = dynamicResult.Data.Id }, dynamicResult);
        }
        else
        {
            return BadRequest(dynamicResult);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateTask(int id, [FromBody] UpdateTaskItemDto dto)
    {
        var result = _taskService.UpdateTask(id, dto) as dynamic; // Temporary fix
        if (result == null)
            return StatusCode(500, "Unexpected result type from service.");

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
        var result = _taskService.ChangeStatus(id, dto) as dynamic; // Temporary fix
        if (result == null)
            return StatusCode(500, "Unexpected result type from service.");

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
        var result = _taskService.DeleteTask(id) as dynamic; // Temporary fix
        if (result == null)
            return StatusCode(500, "Unexpected result type from service.");

        if (!result.Success)
        {
            return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? NotFound(result)
                : BadRequest(result);
        }

        return Ok(result);
    }
}
