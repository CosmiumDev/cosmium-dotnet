using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Contracts;
using TodoApp.Models;

namespace TodoApp.Controllers;

[ApiController]
[Route("todos")]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository _repository;

    public TodoController(ITodoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        var items = await _repository.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(string id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            return NotFound();
            
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem item)
    {
        var created = await _repository.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> Update(string id, TodoItem item)
    {
        if (id != item.id)
            return BadRequest();

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        var updated = await _repository.UpdateAsync(item);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
