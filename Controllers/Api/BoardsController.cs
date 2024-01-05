using KanbanBoardMvc.Context;
using KanbanBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardMvc.Controllers;

[Route("api/boards")]
public class ApiBoardsControllers : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly KanbanDbContext _context;

    public ApiBoardsControllers(ILogger<HomeController> logger, KanbanDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetBoards()
    {
        var boards = await _context.Boards.ToListAsync();

        return View("~/Views/Api/Routes.cshtml", boards);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBoard([Bind("Title")] BoardModel Model)
    {

        Model.Id = new Guid();

        await _context.Boards.AddAsync(Model);
        await _context.SaveChangesAsync();

        var boards = await _context.Boards.ToListAsync();

        return View("~/Views/Api/Routes.cshtml", boards);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBoard([FromQuery] Guid Id)
    {

        var board = await _context.Boards.SingleOrDefaultAsync(e => e.Id == Id);

        if (board is null) return NotFound();

        _context.Entry(board).State = EntityState.Deleted;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}