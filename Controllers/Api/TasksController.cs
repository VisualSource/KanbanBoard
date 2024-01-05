using KanbanBoardMvc.Context;
using KanbanBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var task = await _context.Tasks.SingleOrDefaultAsync(e => e.Id == Id);

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
        var task = await _context.Tasks.SingleOrDefaultAsync(e => e.Id == Id);

        if (task is null)
        {
            return NotFound();
        }


        return View("~/Views/Api/EditTask.cshtml", task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTask([Bind("Title,Description,StatusId")] TaskModel Model)
    {
        Model.Id = new Guid();
        await _context.Tasks.AddAsync(Model);
        await _context.SaveChangesAsync();

        var status = await _context.Status.SingleAsync(e => e.Id == Model.StatusId);

        await _context.Entry(status).Collection(e => e.Tasks).Query().OrderBy(e => e.Order).ToListAsync();
        return View("~/Views/Api/Column.cshtml", status);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid Id)
    {
        var task = await _context.Tasks.SingleAsync(e => e.Id == Id);

        _context.Entry(task).State = EntityState.Deleted;

        await _context.SaveChangesAsync();

        var status = await _context.Status.SingleAsync(e => e.Id == task.StatusId);
        await _context.Entry(status).Collection(e => e.Tasks).Query().OrderBy(e => e.Order).ToListAsync();

        return View("~/Views/Api/Column.cshtml", status);
    }

    [HttpPatch]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PatchTask([Bind("Title,Id,Description")] TaskModel Model)
    {
        var task = await _context.Tasks.SingleOrDefaultAsync(e => e.Id == Model.Id);

        if (task is null)
        {
            return NotFound();
        }

        task.Title = Model.Title;
        task.Description = Model.Description;

        _context.Entry(task).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        var status = await _context.Status.SingleAsync(e => e.Id == task.StatusId);
        await _context.Entry(status).Collection(e => e.Tasks).Query().OrderBy(e => e.Order).ToListAsync();

        return View("~/Views/Api/Column.cshtml", status);
    }
}