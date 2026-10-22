using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Data;
using TaskModel = To_Do_List.Data.Models.Task;

namespace To_Do_List.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetOneTask([FromRoute] int id)
        {
            var result = await _context.Tasks.SingleOrDefaultAsync(task => task.Id == id);
            Console.WriteLine(result);
            if (result == null)
            {
                return NotFound("Task not found");
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTask(TaskModel task)
        {
            var result = await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            Console.WriteLine(result);
            return Ok(task);
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewTask(string name)
        {
            TaskModel task = new() { Name = name };
            await _context.Tasks.AddAsync(task);
            _context.SaveChanges();
            return Ok(task);
        }

        [HttpPut("update")]
        public async Task<IActionResult> EditTask(TaskModel task)
        {
            var t = await _context.Tasks.SingleOrDefaultAsync(x => x.Id == task.Id);
            if (t == null)
            {
                return NotFound("Item Not Found");
            }
            t.Name = task.Name;
            await _context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveTask(int id)
        {
            var t = await _context.Tasks.SingleOrDefaultAsync(x => x.Id == id);
            if (t == null)
            {
                return NotFound($"Task Id {id} not exist");
            }
            _context.Tasks.Remove(t);
            await _context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> PatchTask([FromRoute] int id, [FromBody] JsonPatchDocument<TaskModel> patchDocument)
        {
            var t = await _context.Tasks.SingleOrDefaultAsync(x => x.Id == id);
            if (t == null)
            {
                return NotFound("Task Not Found");
            }
            patchDocument.ApplyTo(t);
            await _context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpGet("getalldata")]
        public async Task<IActionResult> GetAllData()
        {
            var result = _context.Tasks.Count();
            return Ok(result);
        }
    }
}