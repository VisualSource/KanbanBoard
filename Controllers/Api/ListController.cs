using KanbanBoardMvc.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardMvc.Controllers;

[Route("api/list")]
public class ListController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly KanbanDbContext _context;

    public ListController(ILogger<HomeController> logger, KanbanDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    [Route("columns")]
    [HttpPatch]
    public async Task<IActionResult> SortColumns([FromForm(Name = "Column")] List<Guid> guids)
    {

        for (var i = 0; i < guids.Count(); i++)
        {
            var id = guids[i];

            var item = await _context.Status.SingleOrDefaultAsync(e => e.Id == id);
            if (item is null) continue;

            item.Order = guids.Count() - i;

            _context.Entry(item).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();


        return Ok();
    }

    [Route("tasks")]
    [HttpPatch]
    public async Task<IActionResult> SortTasks([FromForm(Name = "Task")] List<Guid> guids)
    {


        for (var i = 0; i < guids.Count(); i++)
        {
            var id = guids[i];

            var item = await _context.Tasks.SingleOrDefaultAsync(e => e.Id == id);
            if (item is null) continue;

            item.Order = guids.Count() - i;

            _context.Entry(item).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();


        return Ok();
    }

}