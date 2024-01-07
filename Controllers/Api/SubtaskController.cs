using Microsoft.AspNetCore.Mvc;
using KanbanBoardMvc.Context;
using Microsoft.EntityFrameworkCore;
namespace KanbanBoardMvc.Controllers;

[Route("api/subtasks")]
public class ApiSubtaskController : Controller
{
    private readonly ILogger<ApiSubtaskController> _logger;
    private readonly KanbanDbContext _context;

    public ApiSubtaskController(ILogger<ApiSubtaskController> logger, KanbanDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [Route("{id:guid}")]
    [HttpPatch]
    public async Task<IActionResult> PatchTask(Guid Id)
    {
        var task = await _context.SubTasks.SingleOrDefaultAsync(e => e.Id == Id);

        if (task is null) return NotFound();

        task.State = !task.State;

        _context.Entry(task).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}