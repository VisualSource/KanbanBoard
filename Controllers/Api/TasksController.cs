using KanbanBoardMvc.Context;
using KanbanBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;

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
}