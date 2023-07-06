using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private static List<Task> _tasks = new List<Task>();
    private static int _nextId = 1;

    // 1. Create a new task
    [HttpPost]
    public IActionResult Create(Task task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    // 2. List all tasks created
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_tasks);
    }

    // 3. Get a specific task
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();
        return Ok(task);
    }

    // 4. Delete a specified task
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();
        _tasks.Remove(task);
        return NoContent();
    }

    // 5. Edit the title or completion of a specific task
    [HttpPut("{id}")]
    public IActionResult Update(int id, Task updatedTask)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();
        task.Title = updatedTask.Title;
        task.IsCompleted = updatedTask.IsCompleted;
        return NoContent();
    }

    // 6. Bulk add multiple tasks in one request
    [HttpPost("bulk")]
    public IActionResult CreateBulk(List<Task> tasks)
    {
        foreach (var task in tasks)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
        }
        return CreatedAtAction(nameof(Get), tasks);
    }

    // 7. Bulk delete multiple tasks in one request
    [HttpDelete("bulk")]
    public IActionResult DeleteBulk(List<int> ids)
    {
        _tasks.RemoveAll(t => ids.Contains(t.Id));
        return NoContent();
    }
}
