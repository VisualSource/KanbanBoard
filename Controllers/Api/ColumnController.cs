using KanbanBoardMvc.Context;
using KanbanBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardMvc.Controllers;

[Route("api/columns")]
public class ApiColumnControllers : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly KanbanDbContext _context;

    public ApiColumnControllers(ILogger<HomeController> logger, KanbanDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("options/{id:guid}")]
    public async Task<IActionResult> GetColumns(Guid Id)
    {

        var items = await _context.Status.Where(e => e.BoardId == Id).ToListAsync();

        return View("~/Views/Api/Options.cshtml", items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTask([Bind("Title,Color,BoardId")] StatusModel Model)
    {
        var order = await _context.Status.Where(e => e.BoardId == Model.BoardId).CountAsync();

        Model.Id = new Guid();
        Model.Order = order;
        var status = await _context.Status.AddAsync(Model);
        await _context.SaveChangesAsync();

        return View("~/Views/Api/Column.cshtml", Model);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteColumn([FromQuery] Guid Id)
    {
        var column = await _context.Status.SingleOrDefaultAsync(e => e.Id == Id);
        if (column is null) return NotFound();

        _context.Entry(column).State = EntityState.Deleted;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}