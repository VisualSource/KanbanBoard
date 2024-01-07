using KanbanBoardMvc.Context;
using KanbanBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace KanbanBoardMvc.Controllers;

[Route("api/tasks")]
public class ApiTasksControllers : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly KanbanDbContext _context;

    public ApiTasksControllers(ILogger<HomeController> logger, KanbanDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTask(Guid Id)
    {

        _logger.LogInformation("Task Id: {id}", Id);
        var task = await _context.Tasks.Where(e => e.Id == Id).Include(e => e.SubTasks).SingleOrDefaultAsync();

        if (task is null)
        {
            return NotFound();
        }

        return View("~/Views/Api/ViewTask.cshtml", task);
    }

    [HttpGet("edit")]
    public async Task<IActionResult> GetEditForm([FromQuery] Guid Id)
    {
        _logger.LogInformation("Task Id: {id}", Id);
        var task = await _context.Tasks.Where(e => e.Id == Id).Include(e => e.SubTasks).SingleOrDefaultAsync();

        if (task is null)
        {
            return NotFound();
        }


        return View("~/Views/Api/EditTask.cshtml", task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTask([Bind("Id,Title,Description,StatusId,SubTasks")] TaskModel Model)
    {
        Model.Id = new Guid();
        Model.SubtaskCount = Model.SubTasks.Count();
        await _context.Tasks.AddAsync(Model);

        foreach (var subtask in Model.SubTasks)
        {
            subtask.Id = Guid.NewGuid();
            subtask.TaskId = Model.Id;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("SubTasks: {}", Model.SubTasks.Count());

        var status = await _context.Status.Where(e => e.Id == Model.StatusId).Include(e => e.Tasks.OrderBy(e => e.Order)).SingleAsync();

        return View("~/Views/Api/Column.cshtml", status);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid Id)
    {
        var task = await _context.Tasks.SingleAsync(e => e.Id == Id);

        _context.Entry(task).State = EntityState.Deleted;

        await _context.SaveChangesAsync();

        var status = await _context.Status.Where(e => e.Id == task.StatusId).Include(e => e.Tasks.OrderBy(e => e.Order)).SingleAsync();

        return View("~/Views/Api/Column.cshtml", status);
    }

    [HttpPatch]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PatchTask([Bind("Title,Id,Description,SubTasks")] TaskModel Model)
    {
        var task = await _context.Tasks.SingleOrDefaultAsync(e => e.Id == Model.Id);

        if (task is null)
        {
            return NotFound();
        }

        task.Title = Model.Title;
        task.Description = Model.Description;

        _context.Entry(task).State = EntityState.Modified;

        foreach (var item in Model.SubTasks)
        {
            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
                item.TaskId = task.Id;
                await _context.SubTasks.AddAsync(item);
            }
            else
            {
                var c = await _context.SubTasks.SingleOrDefaultAsync(e => e.Id == item.Id);
                if (c is null) continue;
                c.Title = item.Title;
                _context.Entry(c).State = EntityState.Modified;
            }
        }

        await _context.SaveChangesAsync();

        var status = await _context.Status.Where(e => e.Id == task.StatusId).Include(e => e.Tasks.OrderBy(e => e.Order)).SingleAsync();

        return View("~/Views/Api/Column.cshtml", status);
    }
}