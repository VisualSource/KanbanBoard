using KanbanBoardMvc.Context;
using KanbanBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;

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
        _logger.LogInformation("ID {id}", id);
        if (id is null)
        {
            return NotFound();
        }

        var guid = new Guid(id);

        var board = await _context.Boards.SingleAsync(b => b.Id == guid);
        var status = await _context.Entry(board).Collection(e => e.Statuses).Query().OrderBy(e => e.Order).ToListAsync();

        foreach (var stat in status)
        {
            await _context.Entry(stat).Collection(e => e.Tasks).Query().OrderBy(e => e.Order).ToListAsync();
        }

        return View(board);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}