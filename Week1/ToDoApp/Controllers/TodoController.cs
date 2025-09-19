using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TodoController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserId() => _httpContextAccessor.HttpContext!.User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        var userId = GetUserId();
        return await _context.TodoItems
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todoItem)
    {
        todoItem.UserId = GetUserId();
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTodos), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        var userId = GetUserId();
        var existingTodo = await _context.TodoItems.FindAsync(id);
        if (existingTodo == null || existingTodo.UserId != userId)
        {
            return NotFound();
        }

        existingTodo.Title = todoItem.Title;
        existingTodo.Description = todoItem.Description;
        existingTodo.IsCompleted = todoItem.IsCompleted;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var userId = GetUserId();
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null || todo.UserId != userId)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}