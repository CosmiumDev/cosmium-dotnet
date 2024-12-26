using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Contracts;

public interface ITodoRepository
{
    Task<TodoItem> CreateAsync(TodoItem item);
    Task<TodoItem> GetByIdAsync(string id);
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem> UpdateAsync(TodoItem item);
    Task DeleteAsync(string id);
}
