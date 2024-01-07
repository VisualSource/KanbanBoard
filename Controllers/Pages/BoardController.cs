using KanbanBoardMvc.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardMvc.Controllers;

public class BoardController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly KanbanDbContext _context;

    public BoardController(ILogger<HomeController> logger, KanbanDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [Route("/boards/{id:guid}")]
    public async Task<IActionResult> Index(string? id)
    {
        _logger.LogInformation("Loading Board with guid({id})", id);
        if (id is null)
        {
            return NotFound();
        }

        var guid = new Guid(id);

        var board = await _context.Boards.Where(e => e.Id == guid)
                                        .Include(e => e.Statuses.OrderBy(e => e.Order))
                                        .ThenInclude(e => e.Tasks.OrderBy(e => e.Order)).AsSplitQuery().SingleAsync();

        return View(board);
    }
}