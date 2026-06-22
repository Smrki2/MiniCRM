using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;
using MiniCRM.DTOs;

namespace MiniCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Company)
                .ToListAsync();

            var result = tasks.Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Company)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            var result = new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> CreateTask(TaskItem task)
        {
            task.Company = null;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest();

            var existing = await _context.Tasks.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Title = task.Title;
            existing.DueDate = task.DueDate;
            existing.IsCompleted = task.IsCompleted;
            existing.CompanyId = task.CompanyId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}